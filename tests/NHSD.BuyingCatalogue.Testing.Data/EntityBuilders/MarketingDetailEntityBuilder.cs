using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class MarketingDetailEntityBuilder
    {
        private readonly MarketingDetailEntity _marketingDetailEntity;

        public static MarketingDetailEntityBuilder Create()
        {
            return new MarketingDetailEntityBuilder();
        }

        public MarketingDetailEntityBuilder()
        {
            //Default
            _marketingDetailEntity = new MarketingDetailEntity
            {
                SolutionId = "Sln1"
            };
        }

        public MarketingDetailEntityBuilder WithSolutionId(string solutionId)
        {
            _marketingDetailEntity.SolutionId = solutionId;
            return this;
        }

        public MarketingDetailEntityBuilder WithAboutUrl(string aboutUrl)
        {
            _marketingDetailEntity.AboutUrl = aboutUrl;
            return this;
        }

        public MarketingDetailEntityBuilder WithFeatures(string features)
        {
            _marketingDetailEntity.Features = features;
            return this;
        }

        public MarketingDetailEntityBuilder WithClientApplication(string clientApplication)
        {
            _marketingDetailEntity.ClientApplication = clientApplication;
            return this;
        }

        public MarketingDetailEntityBuilder WithHosting(string hosting)
        {
            _marketingDetailEntity.Hosting = hosting;
            return this;
        }

        public MarketingDetailEntityBuilder WithRoadMap(string roadMap)
        {
            _marketingDetailEntity.RoadMap = roadMap;
            return this;
        }

        public MarketingDetailEntityBuilder WithRoadMapImageUrl(string roadMapImageUrl)
        {
            _marketingDetailEntity.RoadMapImageUrl = roadMapImageUrl;
            return this;
        }

        public MarketingDetailEntity Build()
        {
            return _marketingDetailEntity;
        }
    }
}
