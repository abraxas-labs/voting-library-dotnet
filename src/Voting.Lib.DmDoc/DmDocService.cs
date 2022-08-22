// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.DmDoc.Exceptions;
using Voting.Lib.DmDoc.Extensions;
using Voting.Lib.DmDoc.Models;
using Voting.Lib.DmDoc.Models.Internal;
using Voting.Lib.DmDoc.Serialization;

namespace Voting.Lib.DmDoc;

/// <summary>
/// Implementation of the DmDoc service client.
/// </summary>
public class DmDocService : IDmDocService
{
    private readonly HttpClient _http;
    private readonly IDmDocUrlBuilder _urlBuilder;
    private readonly IDmDocDataSerializer _dataSerializer;
    private readonly ILogger<DmDocService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DmDocService"/> class.
    /// </summary>
    /// <param name="http">The HTTP client.</param>
    /// <param name="urlBuilder">The DmDoc URL builder.</param>
    /// <param name="dataSerializer">The DmDoc data serializer.</param>
    /// <param name="logger">The logger.</param>
    public DmDocService(
        HttpClient http,
        IDmDocUrlBuilder urlBuilder,
        IDmDocDataSerializer dataSerializer,
        ILogger<DmDocService> logger)
    {
        _http = http;
        _urlBuilder = urlBuilder;
        _dataSerializer = dataSerializer;
        _logger = logger;
    }

    /// <inheritdoc />
    public Task<List<Category>> ListCategories(CancellationToken ct = default)
    {
        var url = _urlBuilder.Categories();
        return _http.GetDmDoc<List<Category>>(url, ct);
    }

    /// <inheritdoc />
    public Task<Template> GetTemplate(int id, CancellationToken ct = default)
    {
        var url = _urlBuilder.Template(id);
        return _http.GetDmDoc<Template>(url, ct);
    }

    /// <inheritdoc />
    public Task<List<Template>> ListTemplates(CancellationToken ct = default)
    {
        var url = _urlBuilder.Templates();
        return _http.GetDmDoc<List<Template>>(url, ct);
    }

    /// <inheritdoc />
    public Task<List<Template>> ListTemplates(string category, CancellationToken ct = default)
    {
        var url = _urlBuilder.Templates(category);
        return _http.GetDmDoc<List<Template>>(url, ct);
    }

    /// <inheritdoc />
    public Task<List<Category>> ListTemplateCategories(int templateId, CancellationToken ct = default)
    {
        var url = _urlBuilder.TemplateCategories(templateId);
        return _http.GetDmDoc<List<Category>>(url, ct);
    }

    /// <inheritdoc />
    public Task<List<DataContainer>> ListTemplateDataContainers(
        int templateId,
        bool includeSystemContainer = false,
        bool includeUserContainer = true,
        CancellationToken ct = default)
    {
        var url = _urlBuilder.TemplatesDataContainers(templateId, includeSystemContainer, includeUserContainer);
        return _http.GetDmDoc<List<DataContainer>>(url, ct);
    }

    /// <inheritdoc />
    public Task<Draft> CreateDraft<T>(string templateName, T templateData, CancellationToken ct = default)
        => CreateDraft(templateName, null, templateData, ct);

    /// <inheritdoc />
    public Task<Draft> CreateDraft<T>(int templateId, T templateData, CancellationToken ct = default)
        => CreateDraft(null, templateId, templateData, ct);

    /// <inheritdoc />
    public async Task DeleteDraft(int draftId, CancellationToken ct = default)
    {
        var url = _urlBuilder.Draft(draftId);
        using var response = await _http.DeleteAsync(url, ct).ConfigureAwait(false);
        await response.EnsureSuccessStatusOrThrowDmDocEx().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<byte[]> PreviewDraftAsPdf(int draftId, CancellationToken ct = default)
    {
        var url = _urlBuilder.DraftPreviewAsPdf(draftId);

        try
        {
            // use byte array instead of stream since, all other pdf endpoints return byte array too.
            // other endpoints require the whole pdf in memory anyway since the response is embedded in the json as base64.
            return await _http.GetByteArrayAsync(url, ct).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            throw new DmDocException(exception);
        }
    }

    /// <inheritdoc />
    public Task<byte[]> PreviewAsPdf<T>(int templateId, T templateData, CancellationToken ct = default)
        => WithTemporaryDraft(null, templateId, templateData, d => PreviewDraftAsPdf(d.Id, ct), ct);

    /// <inheritdoc />
    public Task<byte[]> PreviewAsPdf<T>(string templateName, T templateData, CancellationToken ct = default)
        => WithTemporaryDraft(templateName, null, templateData, d => PreviewDraftAsPdf(d.Id, ct), ct);

    /// <inheritdoc />
    public async Task<byte[]> FinishDraftAsPdf(int draftId, CancellationToken ct = default)
    {
        var url = _urlBuilder.DraftFinishAsPdf(draftId);
        var response = await _http.PutDmDoc<FinishEditingResponse>(url, ct).ConfigureAwait(false);
        return response.ResultPdf
            ?? throw new DmDocException("no pdf received");
    }

    /// <inheritdoc />
    public Task<byte[]> FinishAsPdf<T>(int templateId, T templateData, CancellationToken ct = default)
        => WithTemporaryDraft(null, templateId, templateData, d => FinishDraftAsPdf(d.Id, ct), ct);

    /// <inheritdoc />
    public Task<byte[]> FinishAsPdf<T>(string templateName, T templateData, CancellationToken ct = default)
        => WithTemporaryDraft(templateName, null, templateData, d => FinishDraftAsPdf(d.Id, ct), ct);

    private async Task<TResp> WithTemporaryDraft<TDraftData, TResp>(
        string? templateName,
        int? templateId,
        TDraftData templateData,
        Func<Draft, Task<TResp>> action,
        CancellationToken ct = default)
    {
        var draft = await CreateDraft(templateName, templateId, templateData, ct).ConfigureAwait(false);
        try
        {
            return await action(draft).ConfigureAwait(false);
        }
        finally
        {
            await TryDeleteDraft(draft.Id).ConfigureAwait(false);
        }
    }

    private Task<Draft> CreateDraft<T>(string? templateName, int? templateId, T templateData, CancellationToken ct = default)
    {
        var url = _urlBuilder.Drafts();
        var data = _dataSerializer.Serialize(templateData);
        var request = new CreateDraftRequest
        {
            TemplateName = templateName,
            TemplateId = templateId,
            Data = new List<CreateDraftData>
                {
                    new(_dataSerializer.MimeType, data),
                },
        };
        return _http.PostDmDoc<CreateDraftRequest, Draft>(url, request, ct);
    }

    private async Task TryDeleteDraft(int id)
    {
        try
        {
            await DeleteDraft(id).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Cleaning up the draft {Id} failed", id);
        }
    }
}
