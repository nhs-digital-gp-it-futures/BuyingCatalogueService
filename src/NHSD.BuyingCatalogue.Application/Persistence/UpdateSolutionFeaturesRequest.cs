using System.Collections.Generic;
using Newtonsoft.Json;
using NHSD.BuyingCatalogue.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Application.Persistence
{
    internal sealed class UpdateSolutionFeaturesRequest : IUpdateSolutionFeaturesRequest
    {
        public UpdateSolutionFeaturesRequest(string id, IEnumerable<string> features)
        {
            Id = id;
            Features = JsonConvert.SerializeObject(features).ToString();
        }

        public string Id { get; }

        public string Features { get; }
    }
}
