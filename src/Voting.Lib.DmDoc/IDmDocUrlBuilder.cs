// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

namespace Voting.Lib.DmDoc;

/// <summary>
/// Interface for a DmDoc URL builder.
/// </summary>
public interface IDmDocUrlBuilder
{
    /// <summary>
    /// Gets the categories URL.
    /// </summary>
    /// <returns>The categories URL.</returns>
    string Categories();

    /// <summary>
    /// Gets the URL for a specific template.
    /// </summary>
    /// <param name="id">The template ID.</param>
    /// <returns>The template URL.</returns>
    string Template(int id);

    /// <summary>
    /// Gets the templates URL.
    /// </summary>
    /// <returns>The templates URL.</returns>
    string Templates();

    /// <summary>
    /// Gets the URL for templates of a specific category.
    /// </summary>
    /// <param name="category">The category.</param>
    /// <returns>The templates category URL.</returns>
    string Templates(string category);

    /// <summary>
    /// Gets the URL for the categories of a specific template.
    /// </summary>
    /// <param name="templateId">The template ID.</param>
    /// <returns>The URL to the categories of a specific template.</returns>
    string TemplateCategories(int templateId);

    /// <summary>
    /// Gets the URL for the data containers of a template.
    /// </summary>
    /// <param name="templateId">The template ID.</param>
    /// <param name="includeSystemContainer">Whether to include system containers.</param>
    /// <param name="includeUserContainer">Whether to include user containers.</param>
    /// <returns>Returns the URL to the data containers of a template.</returns>
    string TemplatesDataContainers(int templateId, bool includeSystemContainer, bool includeUserContainer);

    /// <summary>
    /// Gets the drafts URL.
    /// </summary>
    /// <returns>Returns the draft URL.</returns>
    string Drafts();

    /// <summary>
    /// Gets the URL of a specific draft.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <returns>Returns the URL of a specific draft.</returns>
    string Draft(int draftId);

    /// <summary>
    /// Gets the URL to preview a draft as PDF.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <returns>Returns the URL to preview a draft as PDF.</returns>
    string DraftPreviewAsPdf(int draftId);

    /// <summary>
    /// Gets the URL to finish a draft as PDF.
    /// </summary>
    /// <param name="draftId">The draft ID.</param>
    /// <returns>Returns the URL to finish a draft as PDF.</returns>
    string DraftFinishAsPdf(int draftId);
}
