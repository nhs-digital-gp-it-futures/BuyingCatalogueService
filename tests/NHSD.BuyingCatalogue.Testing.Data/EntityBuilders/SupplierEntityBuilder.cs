using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SupplierEntityBuilder
    {
        private readonly SupplierEntity _entity;

        public static SupplierEntityBuilder Create()
        {
            return new SupplierEntityBuilder();
        }

        public SupplierEntityBuilder()
        {
            _entity = new SupplierEntity()
            {
                Id = "Sup",
                OrganisationId = Guid.Empty,
                Name = "Supplier"
            };
        }

        public SupplierEntityBuilder WithId(string id)
        {
            _entity.Id = id;
            return this;
        }

        public SupplierEntityBuilder WithOrganisation(Guid id)
        {
            _entity.OrganisationId = id;
            return this;
        }

        public SupplierEntityBuilder WithName(string name)
        {
            _entity.Name = name;
            return this;
        }

        public SupplierEntityBuilder WithSummary(string summary)
        {
            _entity.Summary = summary;
            return this;
        }

        public SupplierEntityBuilder WithSupplierUrl(string supplierUrl)
        {
            _entity.SupplierUrl = supplierUrl;
            return this;
        }

        public SupplierEntityBuilder WithCrmRef(Guid? crmRef)
        {
            _entity.CrmRef = crmRef;
            return this;
        }

        public SupplierEntityBuilder WithLastUpdated(DateTime time)
        {
            _entity.LastUpdated = time;
            return this;
        }

        public SupplierEntityBuilder WithLastUpdatedBy(Guid id)
        {
            _entity.LastUpdatedBy = id;
            return this;
        }

        public SupplierEntity Build()
        {
            return _entity;
        }
    }
}
