using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionDetailEntityBuilder
    {
        private readonly SolutionDetailEntity _solutionDetailEntity;

        public static SolutionDetailEntityBuilder Create()
        {
            return new SolutionDetailEntityBuilder();
        }

        public SolutionDetailEntityBuilder()
        {
            //Default
            _solutionDetailEntity = new SolutionDetailEntity
            {
                SolutionId = "Sln1",
                Id = Guid.NewGuid(),
                LastUpdated = DateTime.Now,
                LastUpdatedBy = Guid.NewGuid()
            };
        }

        public SolutionDetailEntityBuilder WithId(Guid id)
        {
            _solutionDetailEntity.Id = id;
            return this;
        }

        public SolutionDetailEntityBuilder WithSolutionId(string solutionId)
        {
            _solutionDetailEntity.SolutionId = solutionId;
            return this;
        }

        public SolutionDetailEntityBuilder WithPublishedStatusId(int publishedStatusId)
        {
            _solutionDetailEntity.PublishedStatusId = publishedStatusId;
            return this;
        }

        public SolutionDetailEntityBuilder WithFeatures(string features)
        {
            _solutionDetailEntity.Features = features;
            return this;
        }

        public SolutionDetailEntityBuilder WithClientApplication(string clientApplication)
        {
            _solutionDetailEntity.ClientApplication = clientApplication;
            return this;
        }

        public SolutionDetailEntityBuilder WithHosting(string hosting)
        {
            _solutionDetailEntity.Hosting = hosting;
            return this;
        }

        public SolutionDetailEntityBuilder WithImplementationDetail(string implementationDetail)
        {
            _solutionDetailEntity.ImplementationDetail = implementationDetail;
            return this;
        }

        public SolutionDetailEntityBuilder WithRoadMap(string roadMap)
        {
            _solutionDetailEntity.RoadMap = roadMap;
            return this;
        }

        public SolutionDetailEntityBuilder WithRoadMapImageUrl(string roadMapImageUrl)
        {
            _solutionDetailEntity.RoadMapImageUrl = roadMapImageUrl;
            return this;
        }

        public SolutionDetailEntityBuilder WithAboutUrl(string aboutUrl)
        {
            _solutionDetailEntity.AboutUrl = aboutUrl;
            return this;
        }

        public SolutionDetailEntityBuilder WithSummary(string summary)
        {
            _solutionDetailEntity.Summary = summary;
            return this;
        }

        public SolutionDetailEntityBuilder WithFullDescription(string fullDescription)
        {
            _solutionDetailEntity.FullDescription = fullDescription;
            return this;
        }

        public SolutionDetailEntityBuilder WithLastUpdated(DateTime lastUpdated)
        {
            _solutionDetailEntity.LastUpdated = lastUpdated;
            return this;
        }

        public SolutionDetailEntityBuilder WithLastUpdatedBy(Guid lastUpdatedBy)
        {
            _solutionDetailEntity.LastUpdatedBy = lastUpdatedBy;
            return this;
        }

        public SolutionDetailEntity Build()
        {
            return _solutionDetailEntity;
        }
    }
}
