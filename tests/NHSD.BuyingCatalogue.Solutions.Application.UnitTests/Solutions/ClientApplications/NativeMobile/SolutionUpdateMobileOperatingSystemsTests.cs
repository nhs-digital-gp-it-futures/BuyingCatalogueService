using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.NativeMobile.UpdateSolutionMobileOperatingSystems;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Tools;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands.NativeMobile;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions.ClientApplications.NativeMobile
{
    [TestFixture]
    internal sealed class SolutionUpdateMobileOperatingSystemsTests : ClientApplicationTestsBase
    {
        private const string SolutionId = "Sln1";

        [Test]
        public async Task ShouldUpdateOperatingSystems()
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var validationResult = await UpdateOperatingSystems(
                new HashSet<string> { "Windows", "IOS" },
                "Here are the operating systems");

            validationResult.IsValid.Should().BeTrue();

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateSolutionClientApplicationRequest, bool>> match = r => r.SolutionId == SolutionId
                && JToken.Parse(r.ClientApplication)
                    .ReadStringArray("MobileOperatingSystems.OperatingSystems")
                    .ShouldContainOnly(new List<string> { "Windows", "IOS" })
                    .Count() == 2
                && JToken.Parse(r.ClientApplication)
                    .SelectToken("MobileOperatingSystems.OperatingSystemsDescription")
                    .Value<string>() == "Here are the operating systems";

            Context.MockSolutionDetailRepository.Verify(
                r => r.UpdateClientApplicationAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [TestCase(false, false, false)]
        [TestCase(true, false, false)]
        [TestCase(false, true, false)]
        [TestCase(true, true, true)]
        public async Task ShouldUpdateInvalidOperatingSystems(
            bool isOperatingSystemsValid,
            bool isDescriptionValid,
            bool isValid)
        {
            SetUpMockSolutionRepositoryGetByIdAsync("{}");

            var operationSystem = isOperatingSystemsValid
                ? new HashSet<string> { "Windows", "IOS" }
                : new HashSet<string>();

            var description = isDescriptionValid ? "Some Description" : new string('a', 1001);

            var validationResult = await UpdateOperatingSystems(operationSystem, description);
            validationResult.IsValid.Should().Be(isValid);

            if (!isOperatingSystemsValid)
            {
                var results = validationResult.ToDictionary();
                results["operating-systems"].Should().Be("required");
            }

            if (!isDescriptionValid)
            {
                var results = validationResult.ToDictionary();
                results["operating-systems-description"].Should().Be("maxLength");
            }

            if (isValid)
            {
                Context.MockSolutionRepository.Verify(r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()));
            }
            else
            {
                Context.MockSolutionRepository.Verify(
                    r => r.ByIdAsync("Sln1", It.IsAny<CancellationToken>()),
                    Times.Never());
            }
        }

        [Test]
        public void ShouldThrowWhenNotPresent()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateOperatingSystems(new HashSet<string> { "Windows" }, "Desc"));

            Context.MockSolutionRepository.Verify(r => r.ByIdAsync(SolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<ISolutionDetailRepository, Task>> expression = r => r.UpdateClientApplicationAsync(
                It.IsAny<IUpdateSolutionClientApplicationRequest>(),
                It.IsAny<CancellationToken>());

            Context.MockSolutionDetailRepository.Verify(expression, Times.Never());
        }

        private async Task<ISimpleResult> UpdateOperatingSystems(
            HashSet<string> operatingSystems,
            string operatingSystemsDescription = null)
        {
            Expression<Func<IUpdateNativeMobileOperatingSystemsData, bool>> updateNativeMobileOperatingSystemsData = o =>
                o.OperatingSystems == operatingSystems
                && o.OperatingSystemsDescription == operatingSystemsDescription;

            var data = Mock.Of(updateNativeMobileOperatingSystemsData);

            return await Context.UpdateSolutionMobileOperatingSystemsHandler.Handle(
                new UpdateSolutionMobileOperatingSystemsCommand(SolutionId, data),
                CancellationToken.None);
        }
    }
}
