using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAll;
using NHSD.BuyingCatalogue.Application.UnitTests.Data;
using NHSD.BuyingCatalogue.Application.UnitTests.Infrastructure;
using NHSD.BuyingCatalogue.Domain;
using Xunit;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Solutions.Queries
{
	public class GetAllSolutionSummariesQueryHandlerTests
	{
		private readonly Mock<ISolutionRepository> _solutionRepository;
		private readonly IMapper _mapper;

		/// <summary>
		/// Initialises a new instance of the <see cref="GetAllSolutionSummariesQueryHandlerTests"/> class.
		/// </summary>
		public GetAllSolutionSummariesQueryHandlerTests()
		{
			_solutionRepository = new Mock<ISolutionRepository>();
			_mapper = AutoMapperFactory.Create();
		}

		[Fact]
		public async Task Handler_to_list_a_single_solution()
		{
			//ARRANGE
			var solutionTestData = SolutionListTestData.One();

			_solutionRepository.Setup(x => x.ListSolutionSummaryAsync(CancellationToken.None)).Returns(() => Task.FromResult(solutionTestData));

			GetAllSolutionSummariesQueryHandler testObject = new GetAllSolutionSummariesQueryHandler(_solutionRepository.Object, _mapper);

			//ACT
			var result = await testObject.Handle(new GetAllSolutionSummariesQuery(), CancellationToken.None);

			//ASSERT
			Assert.NotNull(result);
			Assert.Single(result.Solutions);
		}

		[Fact]
		public async Task Handler_to_ignore_all_solutions_with_no_capabilities()
		{
			//ARRANGE
			var solutionTestData = SolutionListTestData.OneWithNoCapabilities();

			_solutionRepository.Setup(x => x.ListSolutionSummaryAsync(CancellationToken.None)).Returns(() => Task.FromResult(solutionTestData));

			GetAllSolutionSummariesQueryHandler testObject = new GetAllSolutionSummariesQueryHandler(_solutionRepository.Object, _mapper);

			//ACT
			var result = await testObject.Handle(new GetAllSolutionSummariesQuery(), CancellationToken.None);

			//ASSERT
			Assert.NotNull(result);
			Assert.Empty(result.Solutions);
		}

		[Fact]
		public async Task Handler_to_ignore_all_solutions_with_no_organisation()
		{
			//ARRANGE
			var solutionTestData = SolutionTestData.Default();
			solutionTestData.Organisation = null;

			_solutionRepository.Setup(x => x.ListSolutionSummaryAsync(CancellationToken.None)).Returns(() => Task.FromResult<IEnumerable<Solution>>(new List<Solution> { solutionTestData }));

			GetAllSolutionSummariesQueryHandler testObject = new GetAllSolutionSummariesQueryHandler(_solutionRepository.Object, _mapper);

			//ACT
			var result = await testObject.Handle(new GetAllSolutionSummariesQuery(), CancellationToken.None);

			//ASSERT
			Assert.Null(solutionTestData.Organisation);

			Assert.NotNull(result);
			Assert.NotNull(result.Solutions);
			Assert.Empty(result.Solutions);
		}

		[Fact]
		public async Task Handler_to_ignore_all_solutions_with_no_capabilities_and_no_organisation()
		{
			//ARRANGE
			var solutionTestData = SolutionTestData.DefaultWithNoCapabilites();
			solutionTestData.Organisation = null;

			_solutionRepository.Setup(x => x.ListSolutionSummaryAsync(CancellationToken.None)).Returns(() => Task.FromResult<IEnumerable<Solution>>(new List<Solution> { solutionTestData }));

			GetAllSolutionSummariesQueryHandler testObject = new GetAllSolutionSummariesQueryHandler(_solutionRepository.Object, _mapper);

			//ACT
			var result = await testObject.Handle(new GetAllSolutionSummariesQuery(), CancellationToken.None);

			//ASSERT
			Assert.Null(solutionTestData.Organisation);

			Assert.NotNull(result);
			Assert.NotNull(result.Solutions);
			Assert.Empty(result.Solutions);
		}
	}
}
