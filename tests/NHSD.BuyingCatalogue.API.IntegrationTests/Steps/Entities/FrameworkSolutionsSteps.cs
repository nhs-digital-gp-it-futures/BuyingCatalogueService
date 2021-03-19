using System.Threading.Tasks;
using JetBrains.Annotations;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal sealed class FrameworkSolutionsSteps
    {
        [Given(@"framework solutions exist")]
        public static async Task GivenFrameworkSolutionsExist(Table table)
        {
            foreach (var frameworkSolution in table.CreateSet<FrameworkSolutionsTable>())
            {
                await InsertFrameworkSolutionsAsync(frameworkSolution);
            }
        }

        private static async Task InsertFrameworkSolutionsAsync(FrameworkSolutionsTable table)
        {
            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(table.SolutionId)
                .WithFoundation(table.IsFoundation)
                .WithFrameworkId(table.FrameworkId)
                .Build()
                .InsertAsync();
        }

        [UsedImplicitly(ImplicitUseTargetFlags.Members)]
        private sealed class FrameworkSolutionsTable
        {
            public string SolutionId { get; init; }

            public bool IsFoundation { get; init; }

            public string FrameworkId { get; init; }
        }
    }
}
