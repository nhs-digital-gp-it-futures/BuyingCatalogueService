using System;
using NHSD.BuyingCatalogue.Domain.Entities;

namespace NHSD.BuyingCatalogue.Application.Tests.Solutions.Data
{
	internal static class CapabilityTestData
	{
		internal static Capability Default()
		{
			var id = Guid.NewGuid().ToString();

			return new Capability
			{
				Id = id,
				Name = $"Capability {id}",
				Description = $"Capability Description {id}",
			};
		}
	}
}
