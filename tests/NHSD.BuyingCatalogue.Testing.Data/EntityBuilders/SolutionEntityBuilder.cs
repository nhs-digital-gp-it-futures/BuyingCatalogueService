using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionEntityBuilder
    {
        private readonly SolutionEntity solutionEntity;

        public SolutionEntityBuilder()
        {
            const string id = "SolutionId";

            solutionEntity = new SolutionEntity
            {
                SolutionId = id,
                Version = "1.0.0",
                ServiceLevelAgreement = null,
                WorkOfPlan = null
            };
        }

        public static SolutionEntityBuilder Create()
        {
            return new();
        }

        public SolutionEntityBuilder WithId(string id)
        {
            solutionEntity.SolutionId = id;
            return this;
        }

        public SolutionEntityBuilder WithFeatures(string features)
        {
            solutionEntity.Features = features;
            return this;
        }

        public SolutionEntityBuilder WithClientApplication(string clientApplication)
        {
            solutionEntity.ClientApplication = clientApplication;
            return this;
        }

        public SolutionEntityBuilder WithHosting(string hosting)
        {
            solutionEntity.Hosting = hosting;
            return this;
        }

        public SolutionEntityBuilder WithImplementationTimescales(string implementationTimescales)
        {
            solutionEntity.ImplementationDetail = implementationTimescales;
            return this;
        }

        public SolutionEntityBuilder WithRoadMap(string roadMap)
        {
            solutionEntity.RoadMap = roadMap;
            return this;
        }

        public SolutionEntityBuilder WithIntegrationsUrl(string integrationsUrl)
        {
            solutionEntity.IntegrationsUrl = integrationsUrl;
            return this;
        }

        public SolutionEntityBuilder WithAboutUrl(string aboutUrl)
        {
            solutionEntity.AboutUrl = aboutUrl;
            return this;
        }

        public SolutionEntityBuilder WithSummary(string summary)
        {
            solutionEntity.Summary = summary;
            return this;
        }

        public SolutionEntityBuilder WithFullDescription(string fullDescription)
        {
            solutionEntity.FullDescription = fullDescription;
            return this;
        }

        public SolutionEntityBuilder WithOnLastUpdated(DateTime lastUpdated)
        {
            solutionEntity.LastUpdated = lastUpdated;
            return this;
        }

        public SolutionEntityBuilder WithLastUpdated(DateTime lastUpdated)
        {
            solutionEntity.LastUpdated = lastUpdated;
            return this;
        }

        public SolutionEntity Build()
        {
            return solutionEntity;
        }
    }
}
