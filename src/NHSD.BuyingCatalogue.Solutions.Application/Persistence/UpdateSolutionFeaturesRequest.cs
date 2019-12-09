using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence
{
    internal sealed class UpdateSolutionFeaturesRequest : IUpdateSolutionFeaturesRequest
    {
        public UpdateSolutionFeaturesRequest(string id, IEnumerable<string> features)
        {
            SolutionId = id;
            Features = JsonConvert.SerializeObject(features);
        }

        public string SolutionId { get; }

        public string Features { get; }
    }
}
