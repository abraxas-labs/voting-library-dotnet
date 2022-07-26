// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file

using System.Collections.Generic;

namespace Voting.Lib.DmDoc.Models.Internal;

internal class CreateDraftRequest
{
    public int? TemplateId { get; set; }

    public string? TemplateName { get; set; }

    public List<CreateDraftData>? Data { get; set; }
}
