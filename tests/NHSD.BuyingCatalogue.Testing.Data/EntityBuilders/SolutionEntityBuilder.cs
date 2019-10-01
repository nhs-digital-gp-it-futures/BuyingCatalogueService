using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionEntityBuilder
    {
        private readonly SolutionEntity _SolutionEntity;

        public static SolutionEntityBuilder Create()
        {
            return new SolutionEntityBuilder();
        }

        public SolutionEntityBuilder()
        {
            //Default
            var id = "SolutionId";

            _SolutionEntity = new SolutionEntity
            {
                Id = id,
                OrganisationId = "Organisation 1",
                Name = $"Solution Name {id}",
                Version = "1.0.0",
                PublishedStatusId = 1,
                AuthorityStatusId = 1,
                SupplierStatusId = 1,
                ParentId = null,
                OnCatalogueVersion = 0,
                Summary = $"Solution Summary {id}",
                FullDescription = $"Solution Full Description {id}",
            };
        }

        public SolutionEntityBuilder WithId(string id)
        {
            _SolutionEntity.Id = id;
            return this;
        }

        public SolutionEntityBuilder WithOrganisationId(string organisationId)
        {
            _SolutionEntity.OrganisationId = organisationId;
            return this;
        }

        public SolutionEntityBuilder WithName(string name)
        {
            _SolutionEntity.Name = name;
            return this;
        }

        public SolutionEntityBuilder WithVersion(string version)
        {
            _SolutionEntity.Version = version;
            return this;
        }

        public SolutionEntityBuilder WithPublishedStatusId(int publishedStatusId)
        {
            _SolutionEntity.PublishedStatusId = publishedStatusId;
            return this;
        }

        public SolutionEntityBuilder WithAuthorityStatusId(int authorityStatusId)
        {
            _SolutionEntity.AuthorityStatusId = authorityStatusId;
            return this;
        }

        public SolutionEntityBuilder WithSupplierStatusId(int supplierStatusId)
        {
            _SolutionEntity.SupplierStatusId = supplierStatusId;
            return this;
        }

        public SolutionEntityBuilder WithParentId(string parentId)
        {
            _SolutionEntity.ParentId = parentId;
            return this;
        }

        public SolutionEntityBuilder WithOnCatalogueVersion(int onCatalogueVersion)
        {
            _SolutionEntity.OnCatalogueVersion = onCatalogueVersion;
            return this;
        }

        public SolutionEntityBuilder WithSummary(string summary)
        {
            _SolutionEntity.Summary = summary;
            return this;
        }

        public SolutionEntityBuilder WithFullDescription(string fullDescription)
        {
            _SolutionEntity.FullDescription = fullDescription;
            return this;
        }

        public SolutionEntity Build()
        {
            return _SolutionEntity;
        }
    }
}
