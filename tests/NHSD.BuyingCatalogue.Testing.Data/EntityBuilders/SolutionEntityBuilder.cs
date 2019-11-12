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
                ParentId = null,
                SupplierId = "Sup 1",
                OrganisationId = Guid.NewGuid(),
                SolutionDetailId = null,
                Name = $"Solution Name {id}",
                Version = "1.0.0",
                PublishedStatusId = 1,
                AuthorityStatusId = 1,
                SupplierStatusId = 1,
                OnCatalogueVersion = 0,
                ServiceLevelAgreement = null,
                WorkOfPlan = null,
                LastUpdated = DateTime.Now,
                LastUpdatedBy = Guid.NewGuid()
            };
        }

        public SolutionEntityBuilder WithId(string id)
        {
            _SolutionEntity.Id = id;
            return this;
        }

        public SolutionEntityBuilder WithParentId(string parentId)
        {
            _SolutionEntity.ParentId = parentId;
            return this;
        }

        public SolutionEntityBuilder WithSupplierId(string supplierId)
        {
            _SolutionEntity.SupplierId = supplierId;
            return this;
        }

        public SolutionEntityBuilder WithOrganisationId(Guid organisationId)
        {
            _SolutionEntity.OrganisationId = organisationId;
            return this;
        }

        public SolutionEntityBuilder WithSolutionDetailId(Guid solutionDetailId)
        {
            _SolutionEntity.SolutionDetailId = solutionDetailId;
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

        public SolutionEntityBuilder WithOnCatalogueVersion(int onCatalogueVersion)
        {
            _SolutionEntity.OnCatalogueVersion = onCatalogueVersion;
            return this;
        }

        public SolutionEntityBuilder WithServiceLevelAgreement(string serviceLevelAgreement)
        {
            _SolutionEntity.ServiceLevelAgreement = serviceLevelAgreement;
            return this;
        }

        public SolutionEntityBuilder WithWorkOfPlan(string workOfPlan)
        {
            _SolutionEntity.WorkOfPlan = workOfPlan;
            return this;
        }

        public SolutionEntityBuilder WithOnLastUpdated(DateTime lastUpdated)
        {
            _SolutionEntity.LastUpdated = lastUpdated;
            return this;
        }

        public SolutionEntityBuilder WithLastUpdatedBy(Guid lastUpdatedBy)
        {
            _SolutionEntity.LastUpdatedBy = lastUpdatedBy;
            return this;
        }


        public SolutionEntity Build()
        {
            return _SolutionEntity;
        }
    }
}
