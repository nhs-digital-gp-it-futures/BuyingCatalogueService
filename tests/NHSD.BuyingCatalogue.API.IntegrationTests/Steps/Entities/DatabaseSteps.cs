using System.Threading.Tasks;
using NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Common;
using NHSD.BuyingCatalogue.Testing.Data;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Entities
{
    [Binding]
    internal class DatabaseSteps
    {
        private readonly ScenarioContext _context;
        private readonly Response _response;

        public DatabaseSteps(ScenarioContext context, Response response)
        {
            _context = context;
            _response = response;
        }

        [Given(@"the call to the database to set the field will fail")]
        public static async Task GivenTheCallToTheDatabaseToSetTheFieldFails()
        {
            await Database.DropServerRoleAsync().ConfigureAwait(false);
        }
    }
}
