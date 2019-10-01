using System;
using NHSD.BuyingCatalogue.Domain.Entities;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Data
{
	internal static class CapabilityTestData
	{
		internal static Capability Default()
		{
			var id = Guid.NewGuid();

			return new Capability
			{
				Id = id,
				Name = $"Capability {id}",
				Description = $"Capability Description {id}",
				IsFoundation = false
			};
		}
	}
}
