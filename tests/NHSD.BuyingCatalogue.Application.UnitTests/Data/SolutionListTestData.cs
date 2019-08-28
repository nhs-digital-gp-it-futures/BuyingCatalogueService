using System.Collections.Generic;
using NHSD.BuyingCatalogue.Domain;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Data
{
	internal static class SolutionListTestData
	{
		internal static IEnumerable<Solution> One()
		{
			yield return SolutionTestData.Default();
		}

		internal static IEnumerable<Solution> OneWithNoCapabilities()
		{
			yield return SolutionTestData.DefaultWithNoCapabilites();
		}
	}
}
