using System.Collections.Generic;
using System.Linq;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps
{
    [Binding]
    public sealed class CommonSteps
    {
        [StepArgumentTransformation]
        public List<string> TransformToListOfString(string commaSeparatedList)
        {
            return commaSeparatedList.Split(",").Select(t => t.Trim()).ToList();
        }
    }
}
