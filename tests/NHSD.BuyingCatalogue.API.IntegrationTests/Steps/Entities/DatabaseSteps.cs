using System.Threading.Tasks;
using NHSD.BuyingCatalogue.Testing.Data;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal class DatabaseSteps
    {
        [Given(@"the call to the database to set the field will fail")]
        public static async Task GivenTheCallToTheDatabaseToSetTheFieldFails()
        {
            await Database.DropServerRoleAsync();
        }
    }
}
