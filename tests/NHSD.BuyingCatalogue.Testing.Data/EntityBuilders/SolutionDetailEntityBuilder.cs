using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionDetailEntityBuilder
    {
        private readonly SolutionDetailEntity solutionDetailEntity;

        public SolutionDetailEntityBuilder()
        {
            solutionDetailEntity = new SolutionDetailEntity
            {
                SolutionId = "Sln1",
                PublishedStatusId = 1,
            };
        }

        public static SolutionDetailEntityBuilder Create()
        {
            return new();
        }

        public SolutionDetailEntityBuilder WithSolutionId(string solutionId)
        {
            solutionDetailEntity.SolutionId = solutionId;
            return this;
        }

        public SolutionDetailEntityBuilder WithFeatures(string features)
        {
            solutionDetailEntity.Features = features;
            return this;
        }

        public SolutionDetailEntityBuilder WithClientApplication(string clientApplication)
        {
            solutionDetailEntity.ClientApplication = clientApplication;
            return this;
        }

        public SolutionDetailEntityBuilder WithHosting(string hosting)
        {
            solutionDetailEntity.Hosting = hosting;
            return this;
        }

        public SolutionDetailEntityBuilder WithImplementationTimescales(string implementationTimescales)
        {
            solutionDetailEntity.ImplementationDetail = implementationTimescales;
            return this;
        }

        public SolutionDetailEntityBuilder WithRoadMap(string roadMap)
        {
            solutionDetailEntity.RoadMap = roadMap;
            return this;
        }

        public SolutionDetailEntityBuilder WithIntegrationsUrl(string integrationsUrl)
        {
            solutionDetailEntity.IntegrationsUrl = integrationsUrl;
            return this;
        }

        public SolutionDetailEntityBuilder WithAboutUrl(string aboutUrl)
        {
            solutionDetailEntity.AboutUrl = aboutUrl;
            return this;
        }

        public SolutionDetailEntityBuilder WithSummary(string summary)
        {
            solutionDetailEntity.Summary = summary;
            return this;
        }

        public SolutionDetailEntityBuilder WithFullDescription(string fullDescription)
        {
            solutionDetailEntity.FullDescription = fullDescription;
            return this;
        }

        public SolutionDetailEntityBuilder WithLastUpdated(DateTime lastUpdated)
        {
            solutionDetailEntity.LastUpdated = lastUpdated;
            return this;
        }

        public SolutionDetailEntity Build()
        {
            return solutionDetailEntity;
        }
    }
}
