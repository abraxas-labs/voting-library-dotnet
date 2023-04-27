// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Voting.Lib.DmDoc.Configuration;
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
    private const string DmDocAuthenticationScheme = "DmDoc";
    private readonly HttpClient _http;
    private readonly DmDocConfig _config;
    private readonly IDmDocUserNameProvider _dmDocUserNameProvider;
    private readonly IDmDocUrlBuilder _urlBuilder;
    private readonly IDmDocDataSerializer _dataSerializer;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<DmDocService> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="DmDocService"/> class.
    /// </summary>
    /// <param name="config">The DmDoc configuation.</param>
    /// <param name="userNameProvider">The username provider.</param>
    /// <param name="urlBuilder">The DmDoc URL builder.</param>
    /// <param name="dataSerializer">The DmDoc data serializer.</param>
    /// <param name="httpClientFactory">The http client factory.</param>
    /// <param name="logger">The logger.</param>
    public DmDocService(
        DmDocConfig config,
        IDmDocUserNameProvider userNameProvider,
        IDmDocUrlBuilder urlBuilder,
        IDmDocDataSerializer dataSerializer,
        IHttpClientFactory httpClientFactory,
        ILogger<DmDocService> logger)
    {
        _config = config;
        _dmDocUserNameProvider = userNameProvider;
        _urlBuilder = urlBuilder;
        _dataSerializer = dataSerializer;
        _httpClientFactory = httpClientFactory;
        _logger = logger;

        if (string.IsNullOrWhiteSpace(_config.Token))
        {
            throw new ValidationException("DmDoc token is required");
        }

        if (string.IsNullOrWhiteSpace(_dmDocUserNameProvider.UserName))
        {
            throw new ValidationException("DmDoc username is required");
        }

        _http = CreateClient();
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
    public Task<Draft> CreateDraft<T>(string templateName, T templateData, string? bulkRoot = null, CancellationToken ct = default)
    {
        var draftRequest = new CreateDraftRequest { TemplateName = templateName };
        return CreateDraft(draftRequest, templateData, bulkRoot, ct);
    }

    /// <inheritdoc />
    public Task<Draft> CreateDraft<T>(int templateId, T templateData, string? bulkRoot = null, CancellationToken ct = default)
    {
        var draftRequest = new CreateDraftRequest { TemplateId = templateId };
        return CreateDraft(draftRequest, templateData, bulkRoot, ct);
    }

    /// <inheritdoc />
    public async Task<Draft> GetDraft(int draftId, CancellationToken ct = default)
    {
        var url = _urlBuilder.Draft(draftId);
        return await _http.GetDmDoc<Draft>(url, ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task DeleteDraft(int draftId, CancellationToken ct = default)
    {
        var url = _urlBuilder.Draft(draftId);
        using var response = await _http.DeleteAsync(url, ct).ConfigureAwait(false);
        await response.EnsureSuccessStatusOrThrowDmDocEx().ConfigureAwait(false);
    }

    /// <inheritdoc />
    public async Task<Stream> PreviewDraftAsPdf(int draftId, CancellationToken ct = default)
    {
        var url = _urlBuilder.DraftPreviewAsPdf(draftId);

        try
        {
            return await _http.GetStreamAsync(url, ct).ConfigureAwait(false);
        }
        catch (Exception exception)
        {
            throw new DmDocException(exception);
        }
    }

    /// <inheritdoc />
    public Task<Stream> PreviewAsPdf<T>(int templateId, T templateData, string? bulkRoot = null, CancellationToken ct = default)
        => WithTemporaryDraft(templateId, templateData, bulkRoot, d => PreviewDraftAsPdf(d.Id, ct), ct);

    /// <inheritdoc />
    public Task<Stream> PreviewAsPdf<T>(string templateName, T templateData, string? bulkRoot = null, CancellationToken ct = default)
        => WithTemporaryDraft(templateName, templateData, bulkRoot, d => PreviewDraftAsPdf(d.Id, ct), ct);

    /// <inheritdoc />
    public async Task<Stream> FinishDraftAsPdf(int draftId, CancellationToken ct = default)
    {
        var url = _urlBuilder.DraftFinishAsPdf(draftId);

        using var request = new HttpRequestMessage(HttpMethod.Put, url);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Pdf));

        // Do not dispose the response, as that would in turn dispose the response content stream.
        // Disposing the content stream should clean up all resources, no need to dispose anything of the response here.
        var response = await _http.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct).ConfigureAwait(false);
        await response.EnsureSuccessStatusOrThrowDmDocEx().ConfigureAwait(false);

        return await response.Content.ReadAsStreamAsync(ct).ConfigureAwait(false);
    }

    /// <inheritdoc />
    public Task<Stream> FinishAsPdf<T>(int templateId, T templateData, string? bulkRoot = null, CancellationToken ct = default)
        => WithTemporaryDraft(templateId, templateData, bulkRoot, d => FinishDraftAsPdf(d.Id, ct), ct);

    /// <inheritdoc />
    public Task<Stream> FinishAsPdf<T>(string templateName, T templateData, string? bulkRoot = null, CancellationToken ct = default)
        => WithTemporaryDraft(templateName, templateData, bulkRoot, d => FinishDraftAsPdf(d.Id, ct), ct);

    /// <inheritdoc />
    public Task<Draft> StartAsyncPdfGeneration<T>(
        int templateId,
        T templateData,
        string webhookEndpoint,
        string? bulkRoot = null,
        CancellationToken ct = default)
        => StartAsyncPdfGeneration(templateId, null, templateData, webhookEndpoint, bulkRoot, ct);

    /// <inheritdoc />
    public Task<Draft> StartAsyncPdfGeneration<T>(
        string templateName,
        T templateData,
        string webhookEndpoint,
        string? bulkRoot = null,
        CancellationToken ct = default)
        => StartAsyncPdfGeneration(null, templateName, templateData, webhookEndpoint, bulkRoot, ct);

    /// <inheritdoc />
    public Task<Stream> GetPdfForPrintJob(int printJobId, CancellationToken ct = default)
    {
        var url = _urlBuilder.PrintJobPdf(printJobId);
        return _http.GetStreamAsync(url, ct);
    }

    /// <inheritdoc />
    public Task<List<Brick>> ListBricks(CancellationToken ct = default)
    {
        var url = _urlBuilder.Bricks();
        return _http.GetDmDoc<List<Brick>>(url, ct);
    }

    /// <inheritdoc />
    public Task<List<Brick>> ListBricks(int categoryId, CancellationToken ct = default)
    {
        var url = _urlBuilder.Bricks(categoryId);
        return _http.GetDmDoc<List<Brick>>(url, ct);
    }

    /// <inheritdoc />
    public Task<List<Brick>> ListBricks(string category, CancellationToken ct = default)
    {
        var url = _urlBuilder.Bricks(category);
        return _http.GetDmDoc<List<Brick>>(url, ct);
    }

    /// <inheritdoc />
    public async Task<string> GetBrickContentEditorUrl(int brickId, int brickContentId, CancellationToken ct = default)
    {
        var url = _urlBuilder.BricksContentEditor(brickId, brickContentId);
        var response = await _http.GetDmDoc<BrickContentEditResponse>(url, ct);

        // dmDoc currently returns a case-sensitive path, but the version needs to be lower cased.
        // dmDoc only returns the relative path of the editor.
        return response.EditorUrlV3.ToLowerInvariant();
    }

    /// <inheritdoc />
    public async Task<(int NewBrickId, int NewContentId)> UpdateBrickContent(int brickContentId, string content, CancellationToken ct = default)
    {
        var url = _urlBuilder.BrickContentUpdate(brickContentId);
        var response = await _http.PutDmDoc<UpdateBrickContentRequest, UpdateBrickContentResponse>(
            url,
            new UpdateBrickContentRequest
            {
                Checkin = true, // creates a new version and thus a new brick id and content id.
                Content = content,
            },
            ct);
        return (response.BrickId, response.Id);
    }

    private Task<TResp> WithTemporaryDraft<TDraftData, TResp>(
        int templateId,
        TDraftData templateData,
        string? bulkRoot,
        Func<Draft, Task<TResp>> action,
        CancellationToken ct)
    {
        var draftRequest = new CreateDraftRequest { TemplateId = templateId };
        return WithTemporaryDraft(draftRequest, templateData, bulkRoot, action, ct);
    }

    private Task<TResp> WithTemporaryDraft<TDraftData, TResp>(
        string templateName,
        TDraftData templateData,
        string? bulkRoot,
        Func<Draft, Task<TResp>> action,
        CancellationToken ct)
    {
        var draftRequest = new CreateDraftRequest { TemplateName = templateName };
        return WithTemporaryDraft(draftRequest, templateData, bulkRoot, action, ct);
    }

    private async Task<TResp> WithTemporaryDraft<TDraftData, TResp>(
        CreateDraftRequest draftRequest,
        TDraftData templateData,
        string? bulkRoot,
        Func<Draft, Task<TResp>> action,
        CancellationToken ct)
    {
        var draft = await CreateDraft(draftRequest, templateData, bulkRoot, ct).ConfigureAwait(false);
        try
        {
            // Serial letter drafts are not being generated immediately, they take some time
            if (!string.IsNullOrEmpty(bulkRoot))
            {
                await PollUntilDraftReady(draft.Id, ct).ConfigureAwait(false);
            }

            return await action(draft).ConfigureAwait(false);
        }
        finally
        {
            await TryDeleteDraft(draft.Id).ConfigureAwait(false);
        }
    }

    private Task<Draft> CreateDraft<T>(CreateDraftRequest draftRequest, T templateData, string? bulkRoot, CancellationToken ct)
    {
        var url = _urlBuilder.Drafts();
        var data = _dataSerializer.Serialize(templateData);
        draftRequest.Data = new List<CreateDraftData>
        {
            new(_dataSerializer.MimeType, data, bulkRoot),
        };
        return _http.PostDmDoc<CreateDraftRequest, Draft>(url, draftRequest, ct);
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

    private Task<Draft> StartAsyncPdfGeneration<T>(
        int? templateId,
        string? templateName,
        T templateData,
        string webhookEndpoint,
        string? bulkRoot,
        CancellationToken ct)
    {
        var draftRequest = new CreateDraftRequest
        {
            TemplateId = templateId,
            TemplateName = templateName,
            Async = true,
            CallbackUrl = webhookEndpoint,
            CallbackActions = new[] { CallbackAction.CreateError, CallbackAction.FinishEditing, CallbackAction.FinishEditingError },
            FinishEditing = new FinishEditingData(),
        };
        return CreateDraft(draftRequest, templateData, bulkRoot, ct);
    }

    private async Task PollUntilDraftReady(int draftId, CancellationToken ct)
    {
        for (var attempt = 0; attempt < _config.MaxDraftStatePollingAttempts; attempt++)
        {
            var draft = await GetDraft(draftId, ct).ConfigureAwait(false);

            switch (draft.State)
            {
                case DraftState.Error:
                    throw new DmDocException("Draft is in error state");
                case DraftState.Editing:
                    // Draft is ready
                    return;
            }

            await Task.Delay(_config.DraftStatePollingDelay, ct).ConfigureAwait(false);
        }

        throw new DmDocException($"Draft was not ready after {_config.MaxDraftStatePollingAttempts} polling attempts");
    }

    private HttpClient CreateClient()
    {
        // IHttpClientFactory.CreateClient() creates a new HttpClient each time, but reuses the internal HttpClientHandler
        // which caches data such as DNS (which improves performance). The created HttpClient does not need to be disposed.
        // See https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests#httpclient-and-lifetime-management
        // Since we only modify the created HttpClient and not the HttpClientHandler, this is a safe approach.
        var httpClient = _httpClientFactory.CreateClient();
        httpClient.Timeout = _config.Timeout ?? Timeout.InfiniteTimeSpan;
        httpClient.BaseAddress = _config.BaseAddress ?? throw new ValidationException("DmDoc base address is required");

        var authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_dmDocUserNameProvider.UserName}:{_config.Token}"));
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(DmDocAuthenticationScheme, authValue);

        return httpClient;
    }
}
