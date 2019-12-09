using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data.EntityBuilders;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    public sealed class FrameworkSolutionsSteps
    {
        [Given(@"Framework Solutions exist")]
        public async Task GivenFrameworkSolutionsExist(Table table)
        {
            foreach (var frameworkSolution in table.CreateSet<FrameworkSolutionsTable>())
            {
                await InsertFrameworkSolutionsAsync(frameworkSolution).ConfigureAwait(false);
            }
        }

        private async Task InsertFrameworkSolutionsAsync(FrameworkSolutionsTable table)
        {
            await FrameworkSolutionEntityBuilder.Create()
                .WithSolutionId(table.SolutionId)
                .WithFoundation(table.IsFoundation)
                .Build()
                .InsertAsync()
                .ConfigureAwait(false);
        }

        private class FrameworkSolutionsTable
        {
            public string SolutionId { get; set; }

            public bool IsFoundation { get; set; }
        }
    }

}
