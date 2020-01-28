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
            _entity = new SupplierEntity
            {
                Id = "Sup",
                Name = "Supplier",
                LegalName = "Supplier",
                Deleted = false
            };
        }

        public SupplierEntityBuilder WithId(string id)
        {
            _entity.Id = id;
            return this;
        }

        public SupplierEntityBuilder WithName(string name)
        {
            _entity.Name = name;
            return this;
        }

        public SupplierEntityBuilder WithLegalName(string legalName)
        {
            _entity.LegalName = legalName;
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

        public SupplierEntityBuilder WithAddress(string address)
        {
            _entity.Address = address;
            return this;
        }

        public SupplierEntityBuilder WithOdsCode(string odsCode)
        {
            _entity.OdsCode = odsCode;
            return this;
        }

        public SupplierEntityBuilder WithCrmRef(Guid? crmRef)
        {
            _entity.CrmRef = crmRef;
            return this;
        }

        public SupplierEntityBuilder WithDeleted(bool delete)
        {
            _entity.Deleted = delete;
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
