using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Solutions.Persistence
{
    internal sealed class UpdateSolutionFeaturesRequest : IUpdateSolutionFeaturesRequest
    {
        public UpdateSolutionFeaturesRequest(string id, IEnumerable<string> features)
        {
            SolutionId = id;
            Features = JsonConvert.SerializeObject(features).ToString();
        }

        public string SolutionId { get; }

        public string Features { get; }
    }
}
