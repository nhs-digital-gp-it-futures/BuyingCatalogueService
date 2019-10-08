using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using Moq;
using NHSD.BuyingCatalogue.Persistence.Infrastructure;
using NHSD.BuyingCatalogue.Persistence.Repositories;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.Entities;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using System.Threading;
using FluentAssertions;

namespace NHSD.BuyingCatalogue.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionRepositoryTests
    {
        private Mock<IConfiguration> _configuration;

        private SolutionRepository _solutionRepository;

        private readonly Guid Cap1Id = Guid.NewGuid();

        [SetUp]
        public async Task Setup()
        {
            await Database.ClearAsync();

            await OrganisationEntityBuilder.Create()
                .WithName("OrgName1")
                .WithId("Org1")
                .Build()
                .InsertAsync();

            await OrganisationEntityBuilder.Create()
                .WithName("OrgName2")
                .WithId("Org2")
                .Build()
                .InsertAsync();

            await CapabilityEntityBuilder.Create().WithName("Cap1").WithId(Cap1Id).WithDescription("Cap1Desc").Build().InsertAsync();

            _configuration = new Mock<IConfiguration>();
            _configuration.Setup(a => a["ConnectionStrings:BuyingCatalogue"]).Returns(ConnectionStrings.ServiceConnectionString());

            _solutionRepository = new SolutionRepository(new DbConnectionFactory(_configuration.Object));
        }

        [Test]
        public async Task ShouldListSingleSolution()
        {
            var organisations = await OrganisationEntity.FetchAllAsync();

            await SolutionEntityBuilder.Create()
                .WithName("Solution1")
                .WithId("Sln1")
                .WithSummary("Sln1Summary")
                .WithOrganisationId(organisations.First(o => o.Name == "OrgName1").Id)
                .Build()
                .InsertAsync();

            await SolutionCapabilityEntityBuilder.Create()
                .WithSolutionId("Sln1")
                .WithCapabilityId(Cap1Id)
                .Build()
                .InsertAsync();

            var solutions = await _solutionRepository.ListAsync(new CancellationToken());

            var solution = solutions.Should().ContainSingle().Subject;
            solution.SolutionId.Should().Be("Sln1");
        }


     }

//    var solutions = await SolutionEntity.FetchAllAsync();
//    var capabilities = await CapabilityEntity.FetchAllAsync();

    //

}
