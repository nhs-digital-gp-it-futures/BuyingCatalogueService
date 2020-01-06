using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.NativeMobile.UpdateSolutionMobileConnectionDetails;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateMobileConnectionDetailsTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateConnectionDetails()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateConnectionDetails(
                new HashSet<string>() { "Pigeon" }, "A description", "1GBps"
                ).ConfigureAwait(false);

            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()), Times.Once);
            Context.MockSolutionDetailRepository.Verify(r => r.UpdateClientApplicationAsync(It.Is<IUpdateSolutionClientApplicationRequest>(r =>
                r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication).ReadStringArray("MobileConnectionDetails.ConnectionType").ShouldContainOnly(new List<string> { "Pigeon" }).Count() == 1
                && JToken.Parse(r.ClientApplication).SelectToken("MobileConnectionDetails.Description").Value<string>() == "A description"
                && JToken.Parse(r.ClientApplication).SelectToken("MobileConnectionDetails.MinimumConnectionSpeed").Value<string>() == "1GBps"
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [TestCase(false, false)]
        [TestCase(true, true)]
        public async Task ShouldNotUpdateInvalidDescription(bool isDescriptionValid, bool isValid)
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var description = isDescriptionValid ? "Some Description" : new string('a', 1001);

            var validationResult = await UpdateConnectionDetails(new HashSet<string>() { "Pigeon" }, description, "1GBps").ConfigureAwait(false);
            validationResult.IsValid.Should().Be(isValid);

            if (!isDescriptionValid)
            {
                validationResult.ToDictionary()["connection-requirements-description"].Should().Be("maxLength");
            }

            if (isValid)
            {
                Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()), Times.Once());

            }
            else
            {
                Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()),
                    Times.Never);
            }
        }

        [Test]
        public void ShouldThrowWhenNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() =>
                UpdateConnectionDetails(new HashSet<string>() { "Windows" }, "Desc", "1GBps"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()),
                Times.Once);

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                    It.IsAny<CancellationToken>()), Times.Never());

        }

        [Test]
        public void CommandShouldTrimStrings()
        {
            var originalViewModel = new UpdateSolutionMobileConnectionDetailsViewModel();
            originalViewModel.ConnectionRequirementsDescription = "   description  ";
            originalViewModel.MinimumConnectionSpeed = @"
                                                    1GBps     ";
            originalViewModel.ConnectionType = new HashSet<string>{"    3G", "      4G    ", "   "};
            var command = new UpdateSolutionMobileConnectionDetailsCommand("Sln1", originalViewModel);
            command.Data.ConnectionRequirementsDescription.Should().Be("description");
            command.Data.MinimumConnectionSpeed.Should().Be("1GBps");
            command.Data.ConnectionType.Should().BeEquivalentTo("3G", "4G");
        }

        private async Task<ISimpleResult> UpdateConnectionDetails(HashSet<string> connectionType, string description, string connectionSpeed)
        {
            return await Context.UpdateSolutionMobileConnectionDetailsHandler.Handle(
                new UpdateSolutionMobileConnectionDetailsCommand(SolutionId,
                    new UpdateSolutionMobileConnectionDetailsViewModel()
                    {
                        ConnectionRequirementsDescription = description,
                        ConnectionType = connectionType,
                        MinimumConnectionSpeed = connectionSpeed
                    }), CancellationToken.None).ConfigureAwait(false);
        }
    }
}
