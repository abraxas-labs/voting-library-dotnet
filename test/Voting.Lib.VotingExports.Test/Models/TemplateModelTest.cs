// (c) Copyright by Abraxas Informatik AG
// For license information see LICENSE file

using System.ComponentModel.DataAnnotations;
using Voting.Lib.VotingExports.Models;
using Xunit;

namespace Voting.Lib.VotingExports.Test.Models;

public class TemplateModelTest
{
    [Fact]
    public void ValidateShouldWorkIfPerDomainOfInfluenceAndMultiple()
    {
        var model = new TemplateModel
        {
            PerDomainOfInfluenceType = true,
            ResultType = ResultType.MultiplePoliticalBusinessesResult,
        };
        model.Validate();
    }

    [Fact]
    public void ValidateShouldThrowIfPerDomainOfInfluenceButNotMultiple()
    {
        var model = new TemplateModel
        {
            PerDomainOfInfluenceType = true,
            ResultType = ResultType.Contest,
        };
        Assert.Throws<ValidationException>(() => model.Validate());
    }
}
