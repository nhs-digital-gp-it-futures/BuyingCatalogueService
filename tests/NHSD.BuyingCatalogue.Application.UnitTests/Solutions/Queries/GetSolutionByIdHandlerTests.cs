using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Solutions;
using NUnit.Framework;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions.Queries
{
    [TestFixture]
    public sealed class GetSolutionByIdHandlerTests
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
        public async Task Handle_CallsRepository_Once()
        {
            //ARRANGE
            var testData = GetMockSolutionResult();

            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).Returns(() => Task.FromResult(testData));

            var testObject = new GetSolutionByIdHandler(new SolutionReader(_repository.Object), _mapper.Object);

            //ACT
            var _ = await testObject.Handle(new GetSolutionByIdQuery(testData.Id), CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.ByIdAsync(testData.Id, CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Handle_CallsMapper_Once()
        {
            //ARRANGE
            var testData = GetMockSolutionResult();

            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).Returns(() => Task.FromResult(testData));

            var testObject = new GetSolutionByIdHandler(new SolutionReader(_repository.Object), _mapper.Object);

            //ACT
            _ = await testObject.Handle(new GetSolutionByIdQuery(testData.Id), CancellationToken.None);

            //ASSERT
            _mapper.Verify(x => x.Map<SolutionByIdViewModel>(It.Is<Solution>(s => s.Id.Equals(testData.Id))), Times.Once);
        }

        [Test]
        public async Task Handle_NoData_ReturnsEmpty()
        {
            //ARRANGE
            var testObject = new GetSolutionByIdHandler(new SolutionReader(_repository.Object), _mapper.Object);

            //ACT
            var result = await testObject.Handle(new GetSolutionByIdQuery("does not exist"), CancellationToken.None);

            //ASSERT
            result.Should().NotBeNull();
            result.Solution.Should().BeNull();
        }

        [Test]
        public async Task Handle_Data_ReturnsData()
        {
            //ARRANGE
            var testData = GetMockSolutionResult();
            var mapRes = new SolutionByIdViewModel();

            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).Returns(() => Task.FromResult(testData));
            _mapper.Setup(x => x.Map<SolutionByIdViewModel>(It.IsAny<Solution>())).Returns(mapRes);

            var testObject = new GetSolutionByIdHandler(new SolutionReader(_repository.Object), _mapper.Object);

            //ACT
            var result = await testObject.Handle(new GetSolutionByIdQuery(testData.Id), CancellationToken.None);

            //ASSERT
            result.Solution.Should().Be(mapRes);
        }

        private ISolutionResult GetMockSolutionResult()
        {
            var result = new Mock<ISolutionResult>();
            result.Setup(r => r.Id).Returns(_id);
            return result.Object;
        }
    }
}
