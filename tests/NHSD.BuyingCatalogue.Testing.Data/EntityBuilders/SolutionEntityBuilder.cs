using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionEntityBuilder
    {
        private readonly SolutionEntity _solutionEntity;

        public static SolutionEntityBuilder Create()
        {
            return new SolutionEntityBuilder();
        }

        public SolutionEntityBuilder()
        {
            //Default
            var id = "SolutionId";

            _solutionEntity = new SolutionEntity
            {
                Id = id,
                ParentId = null,
                SupplierId = "Sup 1",
                SolutionDetailId = null,
                Name = $"Solution Name {id}",
                Version = "1.0.0",
                PublishedStatusId = 1,
                AuthorityStatusId = 1,
                SupplierStatusId = 1,
                OnCatalogueVersion = 0,
                ServiceLevelAgreement = null,
                WorkOfPlan = null
            };
        }

        public SolutionEntityBuilder WithId(string id)
        {
            _solutionEntity.Id = id;
            return this;
        }

        public SolutionEntityBuilder WithParentId(string parentId)
        {
            _solutionEntity.ParentId = parentId;
            return this;
        }

        public SolutionEntityBuilder WithSupplierId(string supplierId)
        {
            _solutionEntity.SupplierId = supplierId;
            return this;
        }

        public SolutionEntityBuilder WithSolutionDetailId(Guid solutionDetailId)
        {
            _solutionEntity.SolutionDetailId = solutionDetailId;
            return this;
        }

        public SolutionEntityBuilder WithName(string name)
        {
            _solutionEntity.Name = name;
            return this;
        }

        public SolutionEntityBuilder WithVersion(string version)
        {
            _solutionEntity.Version = version;
            return this;
        }

        public SolutionEntityBuilder WithPublishedStatusId(int publishedStatusId)
        {
            _solutionEntity.PublishedStatusId = publishedStatusId;
            return this;
        }

        public SolutionEntityBuilder WithAuthorityStatusId(int authorityStatusId)
        {
            _solutionEntity.AuthorityStatusId = authorityStatusId;
            return this;
        }

        public SolutionEntityBuilder WithSupplierStatusId(int supplierStatusId)
        {
            _solutionEntity.SupplierStatusId = supplierStatusId;
            return this;
        }

        public SolutionEntityBuilder WithOnCatalogueVersion(int onCatalogueVersion)
        {
            _solutionEntity.OnCatalogueVersion = onCatalogueVersion;
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
