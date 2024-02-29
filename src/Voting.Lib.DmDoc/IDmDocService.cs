// (c) Copyright 2024 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Voting.Lib.DmDoc.Models;

namespace Voting.Lib.DmDoc;

/// <summary>
/// Interface for the DmDoc service.
/// </summary>
public interface IDmDocService
{
    /// <summary>
    /// List categories.
    /// </summary>
    /// <param name="ct">The cancellation tokne.</param>
    /// <returns>Returns a list of all categories.</returns>
    Task<List<Category>> ListCategories(CancellationToken ct = default);

    /// <summary>
    /// Get the template for the specified ID.
    /// </summary>
    /// <param name="id">The template id.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the template.</returns>
    Task<Template> GetTemplate(int id, CancellationToken ct = default);

    /// <summary>
    /// Lists templates.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns all templates.</returns>
    Task<List<Template>> ListTemplates(CancellationToken ct = default);

    /// <summary>
    /// Lists templates for a category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the templates for the category.</returns>
    Task<List<Template>> ListTemplates(string category, CancellationToken ct = default);

    /// <summary>
    /// Lists the categories of a template.
    /// </summary>
    /// <param name="templateId">The template ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the categories of the template.</returns>
    Task<List<Category>> ListTemplateCategories(int templateId, CancellationToken ct = default);

    /// <summary>
    /// Lists the template data containers.
    /// </summary>
    /// <param name="templateId">The template ID.</param>
    /// <param name="includeSystemContainer">Whether to include system containers.</param>
    /// <param name="includeUserContainer">Whether to include user containers.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the data containers.</returns>
    Task<List<DataContainer>> ListTemplateDataContainers(
        int templateId,
        bool includeSystemContainer = false,
        bool includeUserContainer = true,
        CancellationToken ct = default);

    /// <summary>
    /// Creates a draft for a template.
    /// </summary>
    /// <param name="templateId">The template ID.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="bulkRoot">The optional bulk root. Used for serial letters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns the created draft.</returns>
    Task<Draft> CreateDraft<T>(
        int templateId,
        T templateData,
        string? bulkRoot = null,
        CancellationToken ct = default);

    /// <summary>
    /// Creates a draft for a template.
    /// </summary>
    /// <param name="templateName">The template name.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="bulkRoot">The optional bulk root. Used for serial letters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns the created draft.</returns>
    Task<Draft> CreateDraft<T>(
        string templateName,
        T templateData,
        string? bulkRoot = null,
        CancellationToken ct = default);

    /// <summary>
    /// Fetches a draft by its ID.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>The draft.</returns>
    Task<Draft> GetDraft(int draftId, CancellationToken ct = default);

    /// <summary>
    /// Deletes a draft and its corresponding print job softly, leaving it to be managed by cleanup job.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteDraft(int draftId, CancellationToken ct = default);

    /// <summary>
    /// Deletes a draft's content used for generation only. It will not delete the print job and its generated document.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteDraftContent(int draftId, CancellationToken ct = default);

    /// <summary>
    /// Deletes a draft and its corresponding print job directly.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteDraftHard(int draftId, CancellationToken ct = default);

    /// <summary>
    /// Create a PDF preview for a draft.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns a PDF.</returns>
    Task<Stream> PreviewDraftAsPdf(int draftId, CancellationToken ct = default);

    /// <summary>
    /// Preview a template with data as PDF.
    /// </summary>
    /// <param name="templateId">The template ID.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="bulkRoot">The optional bulk root. Used for serial letters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns a PDF.</returns>
    Task<Stream> PreviewAsPdf<T>(int templateId, T templateData, string? bulkRoot = null, CancellationToken ct = default);

    /// <summary>
    /// Preview a template with data as PDF.
    /// </summary>
    /// <param name="templateName">The template name.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="bulkRoot">The optional bulk root. Used for serial letters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns a PDF.</returns>
    Task<Stream> PreviewAsPdf<T>(string templateName, T templateData, string? bulkRoot = null, CancellationToken ct = default);

    /// <summary>
    /// Finish a draft and return it as PDF.
    /// This should only be used for small documents, as otherwise the PDF generation may take a long time.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns a PDF.</returns>
    Task<Stream> FinishDraftAsPdf(int draftId, CancellationToken ct = default);

    /// <summary>
    /// Finish a template and return it as PDF.
    /// This should only be used for small documents, as otherwise the PDF generation may take a long time.
    /// </summary>
    /// <param name="templateId">The template ID.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="bulkRoot">The optional bulk root. Used for serial letters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns a PDF.</returns>
    Task<Stream> FinishAsPdf<T>(int templateId, T templateData, string? bulkRoot, CancellationToken ct = default);

    /// <summary>
    /// Finish a template and return it as PDF.
    /// This should only be used for small documents, as otherwise the PDF generation may take a long time.
    /// </summary>
    /// <param name="templateName">The template name.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="bulkRoot">The optional bulk root. Used for serial letters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns a PDF.</returns>
    Task<Stream> FinishAsPdf<T>(string templateName, T templateData, string? bulkRoot = null, CancellationToken ct = default);

    /// <summary>
    /// Start the asynchronous PDF generation process, which completes via webhooks.
    /// </summary>
    /// <param name="templateId">The template ID.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="webhookEndpoint">The endpoint of the webhook callback (on error or completion).</param>
    /// <param name="bulkRoot">The optional bulk root. Used for serial letters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>The draft that is being generated.</returns>
    Task<Draft> StartAsyncPdfGeneration<T>(
        int templateId,
        T templateData,
        string webhookEndpoint,
        string? bulkRoot,
        CancellationToken ct = default);

    /// <summary>
    /// Start the asynchronous PDF generation process, which completes via webhooks.
    /// </summary>
    /// <param name="templateName">The template name.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="webhookEndpoint">The endpoint of the webhook callback (on error or completion).</param>
    /// <param name="bulkRoot">The optional bulk root. Used for serial letters.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>The draft that is being generated.</returns>
    Task<Draft> StartAsyncPdfGeneration<T>(
        string templateName,
        T templateData,
        string webhookEndpoint,
        string? bulkRoot = null,
        CancellationToken ct = default);

    /// <summary>
    /// Gets the PDF for a finished print job.
    /// </summary>
    /// <param name="printJobId">The ID of the print job.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the finished PDF of the print job.</returns>
    Task<Stream> GetPdfForPrintJob(int printJobId, CancellationToken ct = default);

    /// <summary>
    /// Get all bricks.
    /// </summary>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns all bricks.</returns>
    Task<List<Brick>> ListBricks(CancellationToken ct = default);

    /// <summary>
    /// Get all bricks of a specific category.
    /// </summary>
    /// <param name="categoryId">The category ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns all bricks of a specific category.</returns>
    Task<List<Brick>> ListBricks(int categoryId, CancellationToken ct = default);

    /// <summary>
    /// Get all bricks of a specific category.
    /// </summary>
    /// <param name="category">The category (specified by intern name of the category).</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns all bricks of a specific category.</returns>
    Task<List<Brick>> ListBricks(string category, CancellationToken ct = default);

    /// <summary>
    /// Get a new brick content standalone editor URL.
    /// </summary>
    /// <param name="brickId">The brick ID.</param>
    /// <param name="brickContentId">The brick content ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns a brick content editor url.</returns>
    Task<string> GetBrickContentEditorUrl(int brickId, int brickContentId, CancellationToken ct = default);

    /// <summary>
    /// Update an existing brick content.
    /// </summary>
    /// <param name="brickContentId">The brick content ID.</param>
    /// <param name="content">The brick content.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns the new brick and brick content ID.</returns>
    Task<(int NewBrickId, int NewContentId)> UpdateBrickContent(int brickContentId, string content, CancellationToken ct = default);
}
