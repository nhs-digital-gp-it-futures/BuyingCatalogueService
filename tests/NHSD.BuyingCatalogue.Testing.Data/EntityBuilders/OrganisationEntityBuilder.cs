using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class OrganisationEntityBuilder
    {
        private readonly OrganisationEntity _organisationEntity;

        public static OrganisationEntityBuilder Create()
        {
            return new OrganisationEntityBuilder();
        }

        public OrganisationEntityBuilder()
        {
            //Default
            _organisationEntity = new OrganisationEntity
            {
                Id = Guid.NewGuid(),
                Name = "Organis1"
            };
        }

        public OrganisationEntityBuilder WithId(Guid id)
        {
            _organisationEntity.Id = id;
            return this;
        }

        public OrganisationEntityBuilder WithName(string name)
        {
            _organisationEntity.Name = name;
            return this;
        }

        public OrganisationEntityBuilder WithOdsCode(string odsCode)
        {
            _organisationEntity.OdsCode = odsCode;
            return this;
        }

        public OrganisationEntityBuilder WithPrimaryRoleId(string primaryRoleId)
        {
            _organisationEntity.PrimaryRoleId = primaryRoleId;
            return this;
        }

        public OrganisationEntityBuilder WithCrmRef(Guid crmRef)
        {
            _organisationEntity.CrmRef = crmRef;
            return this;
        }

        public OrganisationEntityBuilder WithAddress(string address)
        {
            _organisationEntity.Address = address;
            return this;
        }
        public OrganisationEntityBuilder WithCatalogueAgreementSigned(bool catalogueAgreementSigned)
        {
            _organisationEntity.CatalogueAgreementSigned = catalogueAgreementSigned;
            return this;
        }
        public OrganisationEntityBuilder WithDeleted(bool deleted)
        {
            _organisationEntity.Deleted = deleted;
            return this;
        }
        public OrganisationEntityBuilder WithLastUpdated(DateTime lastUpdated)
        {
            _organisationEntity.LastUpdated = lastUpdated;
            return this;
        }
        public OrganisationEntityBuilder WithLastUpdatedBy(Guid lastUpdatedBy)
        {
            _organisationEntity.LastUpdatedBy = lastUpdatedBy;
            return this;
        }

        public OrganisationEntity Build()
        {
            return _organisationEntity;
        }
    }
}
