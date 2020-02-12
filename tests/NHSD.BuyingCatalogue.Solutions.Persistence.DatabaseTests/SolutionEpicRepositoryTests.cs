using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NHSD.BuyingCatalogue.Testing.Data;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.DatabaseTests
{
    [TestFixture]
    public class SolutionEpicRepositoryTests
    {
        private const string SupplierId = "Sup 1";
        private const string Solution1Id = "Sln1";

        private Guid _capabilityGuid;

        private ISolutionEpicRepository _solutionEpicRepository;

        [SetUp]
        public async Task Setup()
        {
            _capabilityGuid = Guid.NewGuid();

            await Database.ClearAsync().ConfigureAwait(false);

            await SupplierEntityBuilder.Create()
                .WithId(SupplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await SolutionEntityBuilder.Create()
                .WithId(Solution1Id)
                .WithSupplierId(SupplierId)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            await CapabilityEntityBuilder.Create()
                .WithId(_capabilityGuid)
                .WithName("Cap1")
                .WithDescription("Desc1")
                .WithVersion("1.0")
                .WithSourceUrl("http://cap1.link")
                .WithCapabilityRef("C1")
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);

            TestContext testContext = new TestContext();
            _solutionEpicRepository = testContext.SolutionEpicRepository;
        }

        [Test]
        public async Task NoSolutionsAsync()
        {
            var solutionCapabilityResponse =
                await _solutionEpicRepository.ListSolutionEpicsAsync(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false);

            solutionCapabilityResponse.Should().BeEmpty();
        }


        [Test]
        public async Task ShouldHaveMultipleEpicsAsync()
        {
            var epicDetails = new[]
            {
                new
                {
                    EpicId = "C1E1",
                    EpicName = "Epic 1",
                    EpicCompliancyLevel = EpicEntityBuilder.CompliancyLevel.Must,
                    SolutionEpicStatus = SolutionEpicEntityBuilder.SolutionEpicStatus.Passed
                },
                new
                {
                    EpicId = "C1E2",
                    EpicName = "Epic 2",
                    EpicCompliancyLevel = EpicEntityBuilder.CompliancyLevel.May,
                    SolutionEpicStatus = SolutionEpicEntityBuilder.SolutionEpicStatus.NotEvidenced
                }
            };
            var expectedResult = new List<ISolutionEpicListResult>();
            foreach (var epicDetail in epicDetails)
            {
                await InsertEpicAsync(epicDetail.EpicId, epicDetail.EpicName, _capabilityGuid,
                    epicDetail.EpicCompliancyLevel).ConfigureAwait(false);
                await InsertSolutionEpicAsync(Solution1Id, _capabilityGuid, epicDetail.EpicId,
                    epicDetail.SolutionEpicStatus).ConfigureAwait(false);
                expectedResult.Add(
                        Mock.Of<ISolutionEpicListResult>(e => e.CapabilityId == _capabilityGuid &&
                                                              e.EpicId == epicDetail.EpicId &&
                                                              e.EpicName == epicDetail.EpicName &&
                                                              e.EpicCompliancyLevel ==
                                                              Enum.GetName(typeof(EpicEntityBuilder.CompliancyLevel),
                                                                  epicDetail.EpicCompliancyLevel) &&
                                                              e.IsMet == (epicDetail.SolutionEpicStatus ==
                                                                          SolutionEpicEntityBuilder.SolutionEpicStatus
                                                                              .Passed)));
            }

            var solutionCapabilityResponse =
                (await _solutionEpicRepository.ListSolutionEpicsAsync(Solution1Id, CancellationToken.None)
                    .ConfigureAwait(false)).ToList();
            solutionCapabilityResponse.Count().Should().Be(2);
            solutionCapabilityResponse.Should().BeEquivalentTo(expectedResult,
                options => options.WithoutStrictOrdering().ComparingByMembers<ISolutionEpicListResult>());
        }


        private async Task InsertEpicAsync(string epicId, string epicName, Guid capId,
            EpicEntityBuilder.CompliancyLevel compliancyLevel)
        {
            await EpicEntityBuilder.Create()
                .WithId(epicId)
                .WithName(epicName)
                .WithActive(true)
                .WithCapabilityId(capId)
                .WithCompliancyLevel(compliancyLevel)
                .WithSourceUrl($"http://{epicId}.link")
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }

        private async Task InsertSolutionEpicAsync(string solutionId, Guid capId, string epicId,
            SolutionEpicEntityBuilder.SolutionEpicStatus status)
        {
            await SolutionEpicEntityBuilder.Create()
                .WithSolutionId(solutionId)
                .WithCapabilityId(capId)
                .WithEpicId(epicId)
                .WithStatus(status)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }
    }
}
