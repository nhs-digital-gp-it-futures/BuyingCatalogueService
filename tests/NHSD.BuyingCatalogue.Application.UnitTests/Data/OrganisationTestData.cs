using System;
using NHSD.BuyingCatalogue.Domain.Entities.Organisations;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Data
{
    internal static class OrganisationTestData
	{
		internal static Organisation Default()
		{
			var id = Guid.NewGuid().ToString();

			return new Organisation
			{
				Id = id,
				Name = $"Organisation {id}",
				Summary = $"Organisation Summary {id}"
			};
		}
	}
}
