using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Moq;
using NHSD.BuyingCatalogue.Application.Infrastructure.Authentication;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.ListSolutions;
using NHSD.BuyingCatalogue.Application.UnitTests.Data;
using NHSD.BuyingCatalogue.Domain;
using NUnit.Framework;
using Shouldly;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions.Queries
{
    [TestFixture]
    public sealed class ListSolutionsHandlerTests
    {
        private Mock<IIdentityProvider> _idProvider;
        private Mock<ISolutionRepository> _repository;
        private Mock<IMapper> _mapper;

        [SetUp]
        public void SetUp()
        {
            _idProvider = new Mock<IIdentityProvider>();
            _repository = new Mock<ISolutionRepository>();
            _mapper = new Mock<IMapper>();
        }

        [Test]
        public async Task Handle_CallsRepository_Once()
        {
            //ARRANGE
            var testData = new [] { SolutionTestData.Default(), SolutionTestData.DefaultWithNoCapabilites() };
            var capabilityIdList = new HashSet<string>();

            _repository.Setup(x => x.ListAsync(capabilityIdList, CancellationToken.None)).Returns(() => Task.FromResult<IEnumerable<Solution>>(testData));

            var testObject = new ListSolutionsHandler(_repository.Object, _mapper.Object);

            //ACT
            var _ = await testObject.Handle(new ListSolutionsQuery(_idProvider.Object), CancellationToken.None);

            //ASSERT
            _repository.Verify(x => x.ListAsync(capabilityIdList, CancellationToken.None), Times.Once);
        }

        [Test]
        public async Task Handle_CallsMapper_Once()
        {
            //ARRANGE
            var testData = new [] { SolutionTestData.Default(), SolutionTestData.Default() };
            var capabilityIdList = new HashSet<string>();

            _repository.Setup(x => x.ListAsync(capabilityIdList, CancellationToken.None)).Returns(() => Task.FromResult<IEnumerable<Solution>>(testData));

            var testObject = new ListSolutionsHandler(_repository.Object, _mapper.Object);

            //ACT
            _ = await testObject.Handle(new ListSolutionsQuery(_idProvider.Object), CancellationToken.None);

            //ASSERT
            _mapper.Verify(x => x.Map<IEnumerable<SolutionSummaryViewModel>>(It.Is<IEnumerable<Solution>>(src => Enumerable.SequenceEqual(src, testData))), Times.Once);
        }

        [Test]
        public async Task Handle_NoData_ReturnsEmpty()
        {
            //ARRANGE
            var testObject = new ListSolutionsHandler(_repository.Object, _mapper.Object);

            //ACT
            var result = await testObject.Handle(new ListSolutionsQuery(_idProvider.Object), CancellationToken.None);

            //ASSERT
            result.ShouldNotBeNull();
            result.Solutions.ShouldBeEmpty();
        }

        [Test]
        public async Task Handle_Data_ReturnsData()
        {
            //ARRANGE
            var testData = new [] { SolutionTestData.Default(), SolutionTestData.DefaultWithNoCapabilites() };
            var mapRes = new List<SolutionSummaryViewModel>();
            var capabilityIdList = new HashSet<string>();

            _repository.Setup(x => x.ListAsync(capabilityIdList, CancellationToken.None)).Returns(() => Task.FromResult<IEnumerable<Solution>>(testData));
            _mapper.Setup(x => x.Map<IEnumerable<SolutionSummaryViewModel>>(It.Is<IEnumerable<Solution>>(src => src == testData)))
              .Returns(mapRes);

            var testObject = new ListSolutionsHandler(_repository.Object, _mapper.Object);

            //ACT
            var result = await testObject.Handle(new ListSolutionsQuery(_idProvider.Object), CancellationToken.None);

            //ASSERT
            result.Solutions.ShouldBe(mapRes);
        }
    }
}
