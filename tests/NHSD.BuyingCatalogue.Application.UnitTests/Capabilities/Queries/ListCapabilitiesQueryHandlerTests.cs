using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Moq;
using NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities;
using NHSD.BuyingCatalogue.Application.Persistence;
using NHSD.BuyingCatalogue.Application.UnitTests.Data;
using NHSD.BuyingCatalogue.Application.UnitTests.Infrastructure;
using NHSD.BuyingCatalogue.Domain.Entities;
using Xunit;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Capabilities.Queries
{
	public sealed class ListCapabilitiesQueryHandlerTests
	{
		private readonly Mock<ICapabilityRepository> _capabilityRepository;
		private readonly IMapper _mapper;

		/// <summary>
		/// Initialises a new instance of the <see cref="ListCapabilitiesQueryHandlerTests"/> class.
		/// </summary>
		public ListCapabilitiesQueryHandlerTests()
		{
			_capabilityRepository = new Mock<ICapabilityRepository>();
			_mapper = AutoMapperFactory.Create();
		}

		[Fact]
		public async Task Handler_to_list_all_capabilities()
		{
			//ARRANGE
			var capabilityTestData = CapabilityListTestData.Two();

			_capabilityRepository.Setup(x => x.ListAsync(CancellationToken.None)).Returns(() => Task.FromResult(capabilityTestData));

			ListCapabilitiesQueryHandler testObject = new ListCapabilitiesQueryHandler(_capabilityRepository.Object, _mapper);

			//ACT
			var result = await testObject.Handle(new ListCapabilitiesQuery(), CancellationToken.None);

			//ASSERT
			Assert.NotNull(result);
			Assert.Equal(capabilityTestData.Count(), result.Capabilities.Count());
		}

		[Fact]
		public async Task Handler_to_list_all_when_no_capabilities_exist()
		{
			//ARRANGE
			IEnumerable<Capability> capabilityTestData = null;

			_capabilityRepository.Setup(x => x.ListAsync(CancellationToken.None)).Returns(() => Task.FromResult(capabilityTestData));

			ListCapabilitiesQueryHandler testObject = new ListCapabilitiesQueryHandler(_capabilityRepository.Object, _mapper);

			//ACT
			var result = await testObject.Handle(new ListCapabilitiesQuery(), CancellationToken.None);

			//ASSERT
			Assert.NotNull(result);
			Assert.Empty(result.Capabilities);
		}
	}
}
