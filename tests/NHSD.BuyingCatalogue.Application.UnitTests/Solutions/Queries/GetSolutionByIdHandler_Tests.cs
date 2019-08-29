using AutoMapper;
using Moq;
using NHSD.BuyingCatalogue.Application.Persistence;
using System;
using System.Collections.Generic;
using System.Text;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAll;
using NHSD.BuyingCatalogue.Application.UnitTests.Data;
using NHSD.BuyingCatalogue.Domain;
using NUnit.Framework;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Application.Persistence;
using AutoMapper;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetSolutionById;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions.Queries
{
    [TestFixture]
    public sealed class GetSolutionByIdHandler_Tests
    {
        private Mock<ISolutionRepository> _repository;
        private Mock<IMapper> _mapper;

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
            var testData = SolutionTestData.Default();

            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).Returns(() => Task.FromResult(testData));

            var testObject = new GetSolutionByIdHandler(_repository.Object, _mapper.Object);

            //ACT
            var _ = await testObject.Handle(new GetSolutionByIdQuery(testData.Id), CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.ByIdAsync(testData.Id, CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Handle_CallsMapper_Once()
        {
            //ARRANGE
            var testData = SolutionTestData.Default();

            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).Returns(() => Task.FromResult(testData));

            var testObject = new GetSolutionByIdHandler(_repository.Object, _mapper.Object);

            //ACT
            _ = await testObject.Handle(new GetSolutionByIdQuery(testData.Id), CancellationToken.None);

            //ASSERT
            _mapper.Verify(x => x.Map<SolutionByIdViewModel>(testData), Times.Once);
        }

        [Test]
        public async Task Handle_NoData_ReturnsEmpty()
        {
            //ARRANGE
            var testObject = new GetSolutionByIdHandler(_repository.Object, _mapper.Object);

            //ACT
            var result = await testObject.Handle(new GetSolutionByIdQuery("does not exist"), CancellationToken.None);

            //ASSERT
            result.ShouldNotBeNull();
            result.Solution.ShouldBeNull();
        }

        [Test]
        public async Task Handle_Data_ReturnsData()
        {
            //ARRANGE
            var testData = SolutionTestData.Default();
            var mapRes = new SolutionByIdViewModel();

            _repository.Setup(x => x.ByIdAsync(testData.Id, CancellationToken.None)).Returns(() => Task.FromResult(testData));
            _mapper.Setup(x => x.Map<SolutionByIdViewModel>(testData))
              .Returns(mapRes);

            var testObject = new GetSolutionByIdHandler(_repository.Object, _mapper.Object);

            //ACT
            var result = await testObject.Handle(new GetSolutionByIdQuery(testData.Id), CancellationToken.None);

            //ASSERT
            result.Solution.ShouldBe(mapRes);
        }
    }
}
