using System;
using NHSD.BuyingCatalogue.Domain;

namespace NHSD.BuyingCatalogue.Application.UnitTests.Data
{
	internal static class SolutionTestData
	{
		internal static Solution Default(string solutionId = null)
		{
			Solution solution = CreateDefaultSolution(solutionId);

			foreach (var capability in CapabilityListTestData.One())
			{
				solution.AddCapability(capability);
			}

			return solution;
		}

		internal static Solution DefaultWithNoCapabilites(string solutionId = null)
		{
			return CreateDefaultSolution(solutionId);
		}

        private static Solution CreateDefaultSolution()
        {
            var id = Guid.NewGuid().ToString();

            return CreateDefaultSolution(id);
        }

        private static Solution CreateDefaultSolution(string solutionId)
		{
			return new Solution
			{
				Id = solutionId,
				Name = $"Solution {solutionId}",
				Summary = $"Solution Summary {solutionId}",
				Organisation = OrganisationTestData.Default(),
                Features = "{ \"features\":[\"Feature 1\",\"Feature 2\"]}"
            };
		}
	}
}
