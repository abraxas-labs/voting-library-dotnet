// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.DmDoc.Models.Internal;

/// <summary>
/// The draft creation request for the 'POST /public_api/v1/drafts'.
/// </summary>
internal class CreateDraftRequest
{
    /// <summary>
    /// Gets or sets the 'template_id' of the template which should be used to create the draft.
    /// </summary>
    public int? TemplateId { get; set; }

    /// <summary>
    /// Gets or sets the intern 'template_name' of the template which should be used to create the draft.
    /// </summary>
    public string? TemplateName { get; set; }

    /// <summary>
    /// Gets or sets a list the input data for the generation with the corresponding template.
    /// </summary>
    public List<CreateDraftData>? Data { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the creation of the draft will be processed asynchronously or synchronously.
    /// If set to true, the creation of the draft will be processed asynchronously.
    /// </summary>
    public bool? Async { get; set; }

    /// <summary>
    /// Gets or sets the callback url which will be notified after a draft is finalized.
    /// </summary>
    public string? CallbackUrl { get; set; }

    /// <summary>
    /// Gets or sets the actions which should lead to a callback to the URL specified in the callback_url parameter.
    /// </summary>
    public CallbackAction[]? CallbackActions { get; set; }

    /// <summary>
    /// Gets or sets the optional finish editing.
    /// If you want to finalize a draft directly after creation you may pass the required parameters using this parameter.
    /// Parameter will be sent as 'finish_editing'.
    /// </summary>
    public FinishEditingData? FinishEditing { get; set; }

    /// <summary>
    /// Gets or sets the optional callback retry policy.
    /// Parameter will be sent as 'callback_retry_policy'.
    /// </summary>
    public CallbackRetryData? CallbackRetryPolicy { get; set; }

    /// <summary>
    /// Gets or sets the timeout in seconds for the callback. If no timeout is specified, the default timeout from DmDoc 'callback_url_timeout' or no timeout will be used.
    /// Parameter will be sent as 'callback_timeout'.
    /// </summary>
    public int? CallbackTimeout { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the async job in DmDoc will be marked as failed if the callback fails.
    /// This parameter is only used if the callback_url parameter is specified.
    /// Parameter will be sent as 'fail_async_job_on_callback_failure'.
    /// </summary>
    public bool FailAsyncJobOnCallbackFailure { get; set; }

    /// <summary>
    /// Gets or sets a value indicating the priority level of the printjob. Highest priorty is 1 default value is 10'000.
    /// This parameter is only used prioritized print jobs.
    /// Parameter will be sent as 'async_job_priority'.
    /// </summary>
    public int? AsyncJobPriority { get; set; }
}
