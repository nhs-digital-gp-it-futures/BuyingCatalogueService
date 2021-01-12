using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionEntityBuilder
    {
        private readonly SolutionEntity _solutionEntity;

        public static SolutionEntityBuilder Create()
        {
            return new();
        }

        public SolutionEntityBuilder()
        {
            // Default
            const string id = "SolutionId";

            _solutionEntity = new SolutionEntity
            {
                Id = id,
                Version = "1.0.0",
                ServiceLevelAgreement = null,
                WorkOfPlan = null,
            };
        }

        public SolutionEntityBuilder WithId(string id)
        {
            _solutionEntity.Id = id;
            return this;
        }

        public SolutionEntityBuilder WithVersion(string version)
        {
            _solutionEntity.Version = version;
            return this;
        }

        public SolutionEntityBuilder WithFeatures(string features)
        {
            _solutionEntity.Features = features;
            return this;
        }

        public SolutionEntityBuilder WithClientApplication(string clientApplication)
        {
            _solutionEntity.ClientApplication = clientApplication;
            return this;
        }

        public SolutionEntityBuilder WithHosting(string hosting)
        {
            _solutionEntity.Hosting = hosting;
            return this;
        }

        public SolutionEntityBuilder WithImplementationTimescales(string implementationTimescales)
        {
            _solutionEntity.ImplementationDetail = implementationTimescales;
            return this;
        }

        public SolutionEntityBuilder WithRoadMap(string roadMap)
        {
            _solutionEntity.RoadMap = roadMap;
            return this;
        }

        public SolutionEntityBuilder WithIntegrationsUrl(string integrationsUrl)
        {
            _solutionEntity.IntegrationsUrl = integrationsUrl;
            return this;
        }

        public SolutionEntityBuilder WithAboutUrl(string aboutUrl)
        {
            _solutionEntity.AboutUrl = aboutUrl;
            return this;
        }

        public SolutionEntityBuilder WithSummary(string summary)
        {
            _solutionEntity.Summary = summary;
            return this;
        }

        public SolutionEntityBuilder WithFullDescription(string fullDescription)
        {
            _solutionEntity.FullDescription = fullDescription;
            return this;
        }

        public SolutionEntityBuilder WithServiceLevelAgreement(string serviceLevelAgreement)
        {
            _solutionEntity.ServiceLevelAgreement = serviceLevelAgreement;
            return this;
        }

        public SolutionEntityBuilder WithWorkOfPlan(string workOfPlan)
        {
            _solutionEntity.WorkOfPlan = workOfPlan;
            return this;
        }

        public SolutionEntityBuilder WithOnLastUpdated(DateTime lastUpdated)
        {
            _solutionEntity.LastUpdated = lastUpdated;
            return this;
        }

        public SolutionEntityBuilder WithLastUpdatedBy(Guid lastUpdatedBy)
        {
            _solutionEntity.LastUpdatedBy = lastUpdatedBy;
            return this;
        }

        public SolutionEntity Build()
        {
            return _solutionEntity;
        }
    }
}
