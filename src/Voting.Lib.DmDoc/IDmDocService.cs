// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
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
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns the created draft.</returns>
    Task<Draft> CreateDraft<T>(
        int templateId,
        T templateData,
        CancellationToken ct = default);

    /// <summary>
    /// Creates a draft for a template.
    /// </summary>
    /// <param name="templateName">The template name.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns the created draft.</returns>
    Task<Draft> CreateDraft<T>(
        string templateName,
        T templateData,
        CancellationToken ct = default);

    /// <summary>
    /// Deletes a draft.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task DeleteDraft(int draftId, CancellationToken ct = default);

    /// <summary>
    /// Create a PDF preview for a draft.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns a PDF.</returns>
    Task<byte[]> PreviewDraftAsPdf(int draftId, CancellationToken ct = default);

    /// <summary>
    /// Preview a template with data as PDF.
    /// </summary>
    /// <param name="templateId">The template ID.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns a PDF.</returns>
    Task<byte[]> PreviewAsPdf<T>(int templateId, T templateData, CancellationToken ct = default);

    /// <summary>
    /// Preview a template with data as PDF.
    /// </summary>
    /// <param name="templateName">The template name.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns a PDF.</returns>
    Task<byte[]> PreviewAsPdf<T>(string templateName, T templateData, CancellationToken ct = default);

    /// <summary>
    /// Finish a draft and return it as PDF.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <returns>Returns a PDF.</returns>
    Task<byte[]> FinishDraftAsPdf(int draftId, CancellationToken ct = default);

    /// <summary>
    /// Finish a template and return it as PDF.
    /// </summary>
    /// <param name="templateId">The draft ID.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns a PDF.</returns>
    Task<byte[]> FinishAsPdf<T>(int templateId, T templateData, CancellationToken ct = default);

    /// <summary>
    /// Finish a template and return it as PDF.
    /// </summary>
    /// <param name="templateName">The template name.</param>
    /// <param name="templateData">The template data.</param>
    /// <param name="ct">The cancellation token.</param>
    /// <typeparam name="T">The type of the template data.</typeparam>
    /// <returns>Returns a PDF.</returns>
    Task<byte[]> FinishAsPdf<T>(string templateName, T templateData, CancellationToken ct = default);
}
