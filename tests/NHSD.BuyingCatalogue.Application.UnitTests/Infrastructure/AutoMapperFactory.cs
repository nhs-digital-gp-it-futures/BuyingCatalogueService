using AutoMapper;
using NHSD.BuyingCatalogue.Application.Infrastructure.Mapping;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Infrastructure
{
	internal static class AutoMapperFactory
	{
		internal static IMapper Create()
		{
			var mappingConfiguration = new MapperConfiguration((mapperConfigurationExpression) =>
			{
				mapperConfigurationExpression.AddProfile(new AutoMapperProfile());
			});

			return mappingConfiguration.CreateMapper();
		}
	}
}
