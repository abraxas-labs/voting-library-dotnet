// (c) Copyright 2022 by Abraxas Informatik AG
// For license information see LICENSE file
using System.Linq;
using FluentAssertions;
using Snapper;
using Voting.Lib.VotingExports.Exceptions;
using Voting.Lib.VotingExports.Models;
using Voting.Lib.VotingExports.Repository;
using Voting.Lib.VotingExports.Repository.Basis;
using Xunit;

namespace Voting.Lib.VotingExports.Test.Repository;

public class TemplateRepositoryTest
{
    [Fact]
    public void GetByGeneratorShouldWork()
    {
        TemplateRepository.GetByGenerator(VotingApp.VotingBasis)
            .All(x => x.GeneratedBy == VotingApp.VotingBasis)
            .Should()
            .BeTrue();
    }

    [Fact]
    public void GetByKeyShouldWork()
    {
        var template = TemplateRepository.GetByKey(BasisXmlContestTemplates.Ech0157And0159.Key);
        template.Key.Should().Be(BasisXmlContestTemplates.Ech0157And0159.Key);
        template.ShouldMatchSnapshot();
    }

    [Fact]
    public void GetByKeyShouldWorkWithUnknownKeyShouldThrow()
    {
        Assert.Throws<TemplateNotFoundException>(() => TemplateRepository.GetByKey("unknown"));
    }

    [Fact]
    public void GetByGeneratorAndEntityTypeShouldWork()
    {
        TemplateRepository.GetByGeneratorAndEntityType(VotingApp.VotingAusmittlung, EntityType.Vote)
            .All(x => x.GeneratedBy == VotingApp.VotingAusmittlung && x.EntityType == EntityType.Vote)
            .Should()
            .BeTrue();
    }
}
