using System;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Infrastructure.Exceptions;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateImplementationTimescales;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Solutions.Application.UnitTests.Solutions
{
    [TestFixture]
    internal sealed class SolutionUpdateImplementationTimescalesTests
    {
        private const string ExistingSolutionId = "Sln1";
        private const string InvalidSolutionId = "Sln123";

        private TestContext context;

        [SetUp]
        public void Setup()
        {
            context = new TestContext();
            context.MockSolutionRepository
                .Setup(r => r.CheckExists(ExistingSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            context.MockSolutionRepository
                .Setup(r => r.CheckExists(InvalidSolutionId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);
        }

        [Test]
        public async Task ShouldUpdateSolutionImplementationTimescales()
        {
            const string expected = "an implementation timescales description";

            var validationResult = await UpdateImplementationTimescalesAsync(ExistingSolutionId, expected);

            validationResult.IsValid.Should().BeTrue();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(0);

            context.MockSolutionRepository.Verify(r => r.CheckExists(ExistingSolutionId, It.IsAny<CancellationToken>()));

            Expression<Func<IUpdateImplementationTimescalesRequest, bool>> match = r =>
                r.SolutionId == ExistingSolutionId
                && r.Description == expected;

            context.MockSolutionRepository.Verify(
                r => r.UpdateImplementationTimescalesAsync(It.Is(match), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldValidateSingleMaxLength()
        {
            var validationResult = await UpdateImplementationTimescalesAsync(ExistingSolutionId, new string('a', 1101));

            validationResult.IsValid.Should().BeFalse();
            var results = validationResult.ToDictionary();
            results.Count.Should().Be(1);
            results["description"].Should().Be("maxLength");

            context.MockSolutionRepository.Verify(
                r => r.CheckExists(ExistingSolutionId, It.IsAny<CancellationToken>()),
                Times.Never());

            Expression<Func<ISolutionRepository, Task>> expression = r => r.UpdateImplementationTimescalesAsync(
                It.IsAny<IUpdateImplementationTimescalesRequest>(),
                It.IsAny<CancellationToken>());

            context.MockSolutionRepository.Verify(expression, Times.Never());
        }

        [Test]
        public void ShouldThrowNotFoundExceptionWhenSolutionIsNotFound()
        {
            Assert.ThrowsAsync<NotFoundException>(() => UpdateImplementationTimescalesAsync(InvalidSolutionId, "A description"));
        }

        private async Task<ISimpleResult> UpdateImplementationTimescalesAsync(string solutionId, string url)
        {
            var validationResult = await context.UpdateImplementationTimescalesHandler.Handle(
                new UpdateImplementationTimescalesCommand(solutionId, url),
                CancellationToken.None);

            return validationResult;
        }
    }
}
