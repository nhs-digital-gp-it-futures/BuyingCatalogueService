using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using Newtonsoft.Json.Linq;
using NHSD.BuyingCatalogue.Application.Exceptions;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution;
using NHSD.BuyingCatalogue.Application.UnitTests.Data;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions.Commands
{
    [TestFixture]
    public sealed class UpdateSolutionHandlerTests
    {
        private Mock<ISolutionRepository> _repository;
        private Mock<IMapper> _mapper;
        private string _id = "Id";

        [SetUp]
        public void SetUp()
        {
            _repository = new Mock<ISolutionRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task Handle_CallsRepositoryMethodByIdAsync_Once()
        {
            //ARRANGE
            var testData = GetMockSolutionResult();
            var command = new UpdateSolutionCommand(testData.Id, new UpdateSolutionViewModel { MarketingData = BuildMarketingData() });

            var updatedSolution = SolutionTestData.Default(testData.Id);
            updatedSolution.Features = command.UpdateSolutionViewModel.MarketingData.ToString();

            _mapper.Setup(x => x.Map(command.UpdateSolutionViewModel, It.Is<Solution>(s => s.Id == _id))).Returns(updatedSolution);
            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).ReturnsAsync(() => testData);
            _repository.Setup(x => x.UpdateAsync(It.Is<UpdateSolutionRequest>(solution => solution.Id.Equals(_id)), CancellationToken.None)).Returns(Task.CompletedTask);

            var testObject = new UpdateSolutionHandler(new SolutionReader(_repository.Object), new SolutionUpdater(_repository.Object), _mapper.Object);

            //ACT
            var _ = await testObject.Handle(command, CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.ByIdAsync(testData.Id, CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Handle_CallsRepositoryMethodUpdateAsync_Once()
        {
            //ARRANGE
            var testData = GetMockSolutionResult();
            var command = new UpdateSolutionCommand(testData.Id, new UpdateSolutionViewModel { MarketingData = BuildMarketingData() });

            var updatedSolution = SolutionTestData.Default(testData.Id);
            updatedSolution.Features = command.UpdateSolutionViewModel.MarketingData.ToString();

            _mapper.Setup(x => x.Map(command.UpdateSolutionViewModel, It.Is<Solution>(s => s.Id == _id))).Returns(updatedSolution);
            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).ReturnsAsync(() => testData);
            _repository.Setup(x => x.UpdateAsync(It.Is<UpdateSolutionRequest>(solution => solution.Id.Equals(_id)), CancellationToken.None)).Returns(Task.CompletedTask);

            var testObject = new UpdateSolutionHandler(new SolutionReader(_repository.Object), new SolutionUpdater(_repository.Object), _mapper.Object);

            //ACT
            var _ = await testObject.Handle(command, CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.UpdateAsync(It.Is<UpdateSolutionRequest>(solution => solution.Id.Equals(_id)), CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Handle_CallsMapper_Once()
        {
            //ARRANGE
            var testData = GetMockSolutionResult();
            var command = new UpdateSolutionCommand(testData.Id, new UpdateSolutionViewModel { MarketingData = BuildMarketingData() });

            var updatedSolution = SolutionTestData.Default(testData.Id);
            updatedSolution.Features = command.UpdateSolutionViewModel.MarketingData.ToString();

            _mapper.Setup(x => x.Map(command.UpdateSolutionViewModel, It.Is<Solution>(s => s.Id == _id))).Returns(updatedSolution);
            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).ReturnsAsync(() => testData);
            _repository.Setup(x => x.UpdateAsync(It.Is<UpdateSolutionRequest>(solution => solution.Id.Equals(_id)), CancellationToken.None)).Returns(Task.CompletedTask);

            var testObject = new UpdateSolutionHandler(new SolutionReader(_repository.Object), new SolutionUpdater(_repository.Object), _mapper.Object);

            //ACT
            _ = await testObject.Handle(command, CancellationToken.None);

            //ASSERT
            _mapper.Verify(x => x.Map(command.UpdateSolutionViewModel, It.IsAny<Solution>()), Times.Once);
        }

        [Test]
        public void Handle_SolutionNotFound_ExceptionRaised()
        {
            //ARRANGE
            var testData = GetMockSolutionResult();
            var command = new UpdateSolutionCommand(testData.Id, new UpdateSolutionViewModel { MarketingData = BuildMarketingData() });

            var updatedSolution = SolutionTestData.Default(testData.Id);
            updatedSolution.Features = command.UpdateSolutionViewModel.MarketingData.ToString();

            _mapper.Setup(x => x.Map(command.UpdateSolutionViewModel, It.Is<Solution>(s => s.Id == _id))).Returns(updatedSolution);
            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).ReturnsAsync(() => null);

            var testObject = new UpdateSolutionHandler(new SolutionReader(_repository.Object), new SolutionUpdater(_repository.Object), _mapper.Object);

            //ACT & ASSERT
            var actual = Assert.ThrowsAsync<NotFoundException>(async () => await testObject.Handle(command, CancellationToken.None));

            actual.Should().NotBeNull();
            actual.Message.Should().Be($"Entity named '{nameof(Solution)}' could not be found matching the ID '{testData.Id}'.");
        }

        private static JObject BuildMarketingData()
        {
            return JObject.Parse(@"{ ""id"" : 1 }");
        }


        private ISolutionResult GetMockSolutionResult()
        {
            var result = new Mock<ISolutionResult>();
            result.Setup(r => r.Id).Returns(_id);
            return result.Object;
        }
    }
}
