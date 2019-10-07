using System.Collections.Generic;
using NHSD.BuyingCatalogue.Contracts.Persistence;
using NHSD.BuyingCatalogue.Domain.Entities.Capabilities;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Data
{
    internal static class CapabilityListTestData
	{
		internal static IEnumerable<ICapabilityListResult> One()
		{
			yield return CapabilityTestData.Default();
		}

		internal static IEnumerable<ICapabilityListResult> Two()
		{
			yield return CapabilityTestData.Default();
			yield return CapabilityTestData.Default();
		}
	}
}
