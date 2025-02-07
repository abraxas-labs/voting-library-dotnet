// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.WebUtilities;

namespace Voting.Lib.DmDoc;

/// <summary>
/// The DmDoc URL builder.
/// </summary>
public class DmDocUrlBuilder : IDmDocUrlBuilder
{
    /// <inheritdoc />
    public string Categories()
        => BuildUrl("categories");

    /// <inheritdoc />
    public string Template(int id)
        => BuildUrl($"templates/{id}");

    /// <inheritdoc />
    public string Templates()
        => BuildUrl("templates");

    /// <inheritdoc />
    public string Templates(string category)
        => BuildUrl("templates", ("category", category));

    /// <inheritdoc />
    public string TemplateCategories(int templateId)
        => BuildUrl($"templates/{templateId}/categories");

    /// <inheritdoc />
    public string TemplatesDataContainers(int templateId, bool includeSystemContainer, bool includeUserContainer)
        => BuildUrl(
            $"templates/{templateId}/data_containers",
            ("include_system_container", includeSystemContainer),
            ("include_user_container", includeUserContainer));

    /// <inheritdoc />
    public string Drafts()
        => BuildUrl("drafts");

    /// <inheritdoc />
    public string Draft(int draftId) => BuildUrl($"drafts/{draftId}");

    /// <inheritdoc />
    public string DraftContent(int draftId) => BuildUrl($"drafts/{draftId}/content");

    /// <inheritdoc />
    public string DraftHardDelete(int draftId)
        => BuildUrl($"drafts/{draftId}", ("hard_delete", true));

    /// <inheritdoc />
    public string DraftPreviewAsPdf(int draftId)
        => BuildUrl($"drafts/{draftId}/preview.pdf");

    /// <inheritdoc />
    public string DraftFinishAsPdf(int draftId)
        => BuildUrl($"drafts/{draftId}/finish_editing", ("distribution", "local_pdf"));

    /// <inheritdoc />
    public string Bricks()
        => BuildUrl("bricks", BuildBricksQueryString());

    /// <inheritdoc />
    public string Bricks(int categoryId)
        => BuildUrl("bricks", BuildBricksQueryString(("category_id", categoryId)));

    /// <inheritdoc />
    public string Bricks(string category)
        => BuildUrl("bricks", BuildBricksQueryString(("category_name", category)));

    /// <inheritdoc />
    public string ActiveBricks(string category)
    => BuildUrl("bricks", BuildBricksActiveQueryString(("category_name", category)));

    /// <inheritdoc />
    public string BricksTagReferences()
        => BuildUrl("bricks/tag_references");

    /// <inheritdoc />
    public string BricksContentEditor(int brickId, int brickContentId)
        => BuildUrl($"bricks/{brickId}/brick_contents/{brickContentId}/edit");

    /// <inheritdoc />
    public string BrickContentUpdate(int brickContentId)
        => BuildUrl($"brick_contents/{brickContentId}/update");

    /// <inheritdoc />
    public string PrintJobPdf(int printJobId)
        => BuildUrl($"print_jobs/{printJobId}.pdf");

    private string BuildUrl(string url, params (string, object?)[] query)
    {
        var queryParams = query
            .Where(q => QueryToString(q) != null)
            .ToDictionary(c => c.Item1, c => QueryToString(c.Item2));

        return QueryHelpers.AddQueryString(url, queryParams);
    }

    private string? QueryToString(object? query)
    {
        return query switch
        {
            bool b => b ? "true" : "false", // use lower case since dm doc doesn't accept upper case (default to string of dotnet boolean).
            _ => query?.ToString(),
        };
    }

    private (string, object?)[] BuildBricksQueryString((string Field, object? Value)? param = null)
    {
        var paramList = new List<(string Field, object? Value)>
        {
            ("show_current", false),
            ("show_editable", true),
            ("skip_preview_data", true),
            ("skip_containers", true),
            ("skip_dirty_check", true),
        };

        if (param.HasValue)
        {
            paramList.Add(param.Value);
        }

        return paramList.ToArray();
    }

    private (string, object?)[] BuildBricksActiveQueryString((string Field, object? Value)? param = null)
    {
        var paramList = new List<(string Field, object? Value)>
        {
            ("skip_preview_data", true),
            ("skip_containers", true),
            ("skip_dirty_check", true),
        };

        if (param.HasValue)
        {
            paramList.Add(param.Value);
        }

        return paramList.ToArray();
    }
}
