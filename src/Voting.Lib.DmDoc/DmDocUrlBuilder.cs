// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Text;
using System.Text.Encodings.Web;

namespace Voting.Lib.DmDoc;

/// <summary>
/// The DmDoc URL builder.
/// </summary>
public class DmDocUrlBuilder : IDmDocUrlBuilder
{
    private readonly IDmDocUserNameProvider _userNameProvider;

    /// <summary>
    /// Initializes a new instance of the <see cref="DmDocUrlBuilder"/> class.
    /// </summary>
    /// <param name="userNameProvider">The DmDoc user name provider.</param>
    public DmDocUrlBuilder(IDmDocUserNameProvider userNameProvider)
    {
        _userNameProvider = userNameProvider;
    }

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
    public string DraftPreviewAsPdf(int draftId)
        => BuildUrl($"drafts/{draftId}/preview.pdf");

    /// <inheritdoc />
    public string DraftFinishAsPdf(int draftId)
        => BuildUrl($"drafts/{draftId}/finish_editing", ("distribution", "local_pdf"));

    /// <inheritdoc />
    public string Bricks()
        => BuildUrl("bricks", ("show_current", false), ("show_editable", true));

    /// <inheritdoc />
    public string Bricks(int categoryId)
        => BuildUrl("bricks", ("category_id", categoryId), ("show_current", false), ("show_editable", true));

    /// <inheritdoc />
    public string Bricks(string category)
        => BuildUrl("bricks", ("category_name", category), ("show_current", false), ("show_editable", true));

    /// <inheritdoc />
    public string BricksContentEditor(int brickId, int brickContentId)
        => BuildUrl($"bricks/{brickId}/brick_contents/{brickContentId}/edit");

    /// <inheritdoc />
    public string BrickContentUpdate(int brickContentId)
        => BuildUrl($"brick_contents/{brickContentId}/update");

    private string BuildUrl(string url, params (string, object?)[] query)
    {
        var sb = new StringBuilder(url);
        sb.Append("?user_name=").Append(UrlEncoder.Default.Encode(_userNameProvider.UserName));
        foreach (var (key, value) in query)
        {
            var strValue = QueryToString(value);
            if (strValue == null)
            {
                continue;
            }

            sb.Append('&');
            sb.Append(UrlEncoder.Default.Encode(key));
            sb.Append('=');
            sb.Append(UrlEncoder.Default.Encode(strValue));
        }

        return sb.ToString();
    }

    private string? QueryToString(object? query)
    {
        return query switch
        {
            bool b => b ? "true" : "false", // use lower case since dm doc doesn't accept upper case (default to string of dotnet boolean).
            _ => query?.ToString(),
        };
    }
}
