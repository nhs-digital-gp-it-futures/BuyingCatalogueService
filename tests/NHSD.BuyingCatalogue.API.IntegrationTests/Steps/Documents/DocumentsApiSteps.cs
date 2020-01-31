using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace NHSD.BuyingCatalogue.API.IntegrationTests.Steps.Documents
{
    [Binding]
    internal sealed class DocumentsApiSteps
    {
        [Given(@"a document named ([\w-]*) exists")]
        public void GivenADocumentNamedRoadmapExists(string documentName)
        {
            ScenarioContext.Current.Pending();
        }

    }
}
