// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Xml.Serialization;
using eCH_0157_4_0;
using Voting.Lib.Common;
using Voting.Lib.Ech.Ech0157.Schemas;

namespace Voting.Lib.Ech.Ech0157.Converter;

/// <summary>
/// Deserializer for the eCH-0157.
/// </summary>
public class Ech0157Deserializer
{
    /// <summary>
    /// Reads the <see cref="EventInitialDeliveryType"/>.
    /// </summary>
    /// <param name="xml">The raw xml.</param>
    /// <returns>The deserialized eCH-0157.</returns>
    public EventInitialDeliveryType ReadXml(string xml)
    {
        using var sr = new StringReader(xml);
        var schemaSet = Ech0157Schemas.LoadEch0157Schemas();
        using var reader = XmlUtil.CreateReaderWithSchemaValidation(sr, schemaSet);

        try
        {
            var serializer = new XmlSerializer(typeof(EventInitialDeliveryType));
            return (EventInitialDeliveryType?)serializer.Deserialize(reader)
                ?? throw new ValidationException("Deserialization returned null");
        }
        catch (InvalidOperationException ex) when (ex.InnerException != null)
        {
            // The XmlSerializer wraps all exceptions into an InvalidOperationException.
            // Unwrap it to surface the "correct" exception type.
            throw ex.InnerException;
        }
    }
}
