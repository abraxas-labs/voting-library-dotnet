// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Voting.Lib.Ech.Extensions;

namespace Voting.Lib.Ech;

/// <summary>
/// Provides methods to serialize eCH content.
/// </summary>
public class EchSerializer
{
    private static readonly RecyclableMemoryStreamManager RecyclableMemoryStreamManager = new();

    private static readonly Encoding Encoding = new UTF8Encoding(false);

    private static readonly XmlWriterSettings XmlWriterSettings = new()
    {
        Indent = false,
        NewLineOnAttributes = false,
        Encoding = Encoding,
        CloseOutput = false,
    };

    private static readonly XmlWriterSettings XmlFragmentWriterSettings = new()
    {
        Indent = false,
        NewLineOnAttributes = false,
        Encoding = Encoding,
        Async = true,
        OmitXmlDeclaration = true,
        CloseOutput = false,
    };

    private static readonly XmlReaderSettings XmlReaderSettings = new()
    {
        Async = true,
    };

    private readonly ILogger<EchSerializer> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="EchSerializer"/> class.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    public EchSerializer(ILogger<EchSerializer> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Writes an XML to the provided writer.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="entity">The eCH object to serialize.</param>
    /// <param name="xmlAttributeOverrides">Optional XML attribute overrides (eg. for extensions).</param>
    /// <param name="leaveStreamOpen">Whether to leave the stream open.</param>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    public void WriteXml<T>(PipeWriter writer, T entity, XmlAttributeOverrides? xmlAttributeOverrides = null, bool leaveStreamOpen = false)
        where T : notnull
        => WriteXml(writer.AsStream(), entity, xmlAttributeOverrides, leaveStreamOpen);

    /// <summary>
    /// Writes an XML to the provided stream.
    /// </summary>
    /// <param name="stream">The target stream.</param>
    /// <param name="entity">The eCH object to serialize.</param>
    /// <param name="xmlAttributeOverrides">Optional XML attribute overrides (eg. for extensions).</param>
    /// <param name="leaveStreamOpen">Whether to leave the stream open.</param>
    /// <typeparam name="T">The type of the object to serialize.</typeparam>
    public void WriteXml<T>(Stream stream, T entity, XmlAttributeOverrides? xmlAttributeOverrides = null, bool leaveStreamOpen = false)
    {
        var serializer = new XmlSerializer(typeof(T), xmlAttributeOverrides);
        using var streamWriter = new StreamWriter(stream, Encoding, leaveOpen: leaveStreamOpen);
        using var xmlWriter = XmlWriter.Create(streamWriter, XmlWriterSettings);
        serializer.Serialize(xmlWriter, entity);
    }

    /// <summary>
    /// Writes an XML to the provided writer.
    /// This method can be used to write huge XML data,
    /// if one XML element is repeated several times,
    /// without keeping all elements in memory.
    /// The provided object should include exactly one prototype element,
    /// which then gets replaced with all provided elements.
    /// </summary>
    /// <param name="writer">The writer.</param>
    /// <param name="prototypeElementName">The XML element name of the items.</param>
    /// <param name="o">The object with exactly one prototype element.</param>
    /// <param name="elements">The elements which replace the prototype element.</param>
    /// <param name="xmlAttributeOverrides">Optional XML attribute overrides (eg. for extensions).</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="TRoot">The type of the xml root element.</typeparam>
    /// <typeparam name="TItem">The type of the elements.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation..</returns>
    public Task WriteXmlWithElements<TRoot, TItem>(
        PipeWriter writer,
        XmlQualifiedName prototypeElementName,
        TRoot o,
        IAsyncEnumerable<TItem> elements,
        XmlAttributeOverrides? xmlAttributeOverrides = null,
        CancellationToken ct = default)
        where TItem : class
        => WriteXmlWithElements(writer.AsStream(), prototypeElementName, o, elements, xmlAttributeOverrides, ct);

    /// <summary>
    /// Writes an XML to the provided stream.
    /// This method can be used to write huge XML data,
    /// if one XML element is repeated several times,
    /// without keeping all elements in memory.
    /// The provided object should include exactly one prototype element,
    /// which then gets replaced with all provided elements.
    /// </summary>
    /// <param name="stream">The stream.</param>
    /// <param name="prototypeElementName">The XML element name of the items.</param>
    /// <param name="o">The object with exactly one prototype element.</param>
    /// <param name="elements">The elements which replace the prototype element.</param>
    /// <param name="xmlAttributeOverrides">Optional XML attribute overrides (eg. for extensions).</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="TRoot">The type of the xml root element.</typeparam>
    /// <typeparam name="TItem">The type of the elements.</typeparam>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation..</returns>
    public async Task WriteXmlWithElements<TRoot, TItem>(
        Stream stream,
        XmlQualifiedName prototypeElementName,
        TRoot o,
        IAsyncEnumerable<TItem> elements,
        XmlAttributeOverrides? xmlAttributeOverrides = null,
        CancellationToken ct = default)
        where TItem : class
    {
        using var ms = RecyclableMemoryStreamManager.GetStream();
        WriteXml(ms, o, xmlAttributeOverrides, true);
        ms.Seek(0, SeekOrigin.Begin);
        await CopyAndReplacePrototypeElement(ms, stream, prototypeElementName, elements, xmlAttributeOverrides, ct);
    }

    private async Task CopyAndReplacePrototypeElement<T>(
        Stream prototypeStream,
        Stream target,
        XmlQualifiedName elementName,
        IAsyncEnumerable<T> elements,
        XmlAttributeOverrides? overrides,
        CancellationToken ct)
        where T : class
    {
        overrides ??= new();

        // since the XML root on the eCH generated classes from the eai team do not match
        // the element names used in the eCH object tree we need to override it here
        var xmlRoot = new XmlRootAttribute
        {
            ElementName = elementName.Name,
            Namespace = elementName.Namespace,
        };

        var attributes = overrides[typeof(T)];
        if (attributes == null)
        {
            overrides.Add(typeof(T), new XmlAttributes { XmlRoot = xmlRoot });
        }
        else
        {
            attributes.XmlRoot = xmlRoot;
        }

        var xmlReader = XmlReader.Create(prototypeStream, XmlReaderSettings);
        var readerLineInfo = (IXmlLineInfo)xmlReader;

        // move reader to the prototype element and store the line info
        await xmlReader.MoveToElementAsync(elementName);

        // as we don't indent, we only need to keep track of the line position
        if (readerLineInfo.LineNumber != 1)
        {
            throw new InvalidOperationException("Unexpected line position, indented XML is not supported");
        }

        // -2:
        // -1 our code is 0 indexed, XML code is one indexed
        // -1 we want the cursor before the XML open sign <
        var prototypeStartPosition = readerLineInfo.LinePosition - 2;

        // read the prototype element
        await foreach (var x in xmlReader.EnumerateElementsAsync<T>(elementName, ct))
        {
            break;
        }

        // store how many positions the prototype takes
        // -3:
        // -1 our code is 0 indexed, XML code is one indexed
        // -1 we want the cursor before the XML open sign <
        // -1 since we calculate the diff
        var charsToSkip = readerLineInfo.LinePosition - prototypeStartPosition - 3;

        // restart the prototype stream
        prototypeStream.Seek(0, SeekOrigin.Begin);

        using var prototypeReader = new StreamReader(prototypeStream);
        await using var targetWriter = new StreamWriter(target, Encoding);

        // copy everything before the element to the target stream
        // -1 position, as we want to set the cursor before the element to skip
        await prototypeReader.CopyToAsync(targetWriter, prototypeStartPosition, ct);

        // skip the prototype element
        await prototypeReader.SeekAsync(charsToSkip, ct);

        // serialize all elements
        await SerializeElements(targetWriter, elements, overrides);

        // copy remainder
        await prototypeReader.CopyToAsync(targetWriter, ct);
    }

    private async Task SerializeElements<T>(
        TextWriter targetWriter,
        IAsyncEnumerable<T> elements,
        XmlAttributeOverrides overrides)
    {
        var serializer = new XmlSerializer(typeof(T), overrides);

        await foreach (var element in elements)
        {
            try
            {
                await using var writer = XmlWriter.Create(targetWriter, XmlFragmentWriterSettings);
                serializer.Serialize(writer, element);
            }
            catch (Exception e)
            {
                // log exception here already, since it could be hidden
                // due to the streaming architecture
                _logger.LogError(e, "Could not serialize element");
                throw;
            }
        }
    }
}
