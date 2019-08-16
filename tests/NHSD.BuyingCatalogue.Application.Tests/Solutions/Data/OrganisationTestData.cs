using System;
using NHSD.BuyingCatalogue.Domain.Entities;

namespace NHSD.BuyingCatalogue.Application.Tests.Solutions.Data
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
