using System.Collections.Generic;
using NHSD.BuyingCatalogue.Domain.Entities;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Data
{
	internal static class CapabilityListTestData
	{
		internal static IEnumerable<Capability> One()
		{
			yield return CapabilityTestData.Default();
		}

		internal static IEnumerable<Capability> Two()
		{
			yield return CapabilityTestData.Default();
			yield return CapabilityTestData.Default();
		}
	}
}
