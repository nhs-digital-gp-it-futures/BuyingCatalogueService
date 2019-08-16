using System;
using NHSD.BuyingCatalogue.Domain;

namespace NHSD.BuyingCatalogue.Application.Tests.Solutions.Data
{
	internal static class SolutionTestData
	{
		internal static Solution Default()
		{
			Solution solution = CreateDefaultSolution();

			foreach (var capability in CapabilityListTestData.One())
			{
				solution.AddCapability(capability);
			}

			return solution;
		}

		internal static Solution DefaultWithNoCapabilites()
		{
			return CreateDefaultSolution();
		}

		private static Solution CreateDefaultSolution()
		{
			var id = Guid.NewGuid().ToString();

			return new Solution
			{
				Id = id,
				Name = $"Solution {id}",
				Summary = $"Solution Summary {id}",
				Organisation = OrganisationTestData.Default()
			};
		}
	}
}
