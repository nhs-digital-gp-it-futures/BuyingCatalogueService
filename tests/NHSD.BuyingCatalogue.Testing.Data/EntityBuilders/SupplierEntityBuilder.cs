using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SupplierEntityBuilder
    {
        private readonly SupplierEntity entity;

        public SupplierEntityBuilder()
        {
            entity = new SupplierEntity
            {
                Id = "Sup",
                Name = "Supplier",
                LegalName = "Supplier",
                Deleted = false,
            };
        }

        public static SupplierEntityBuilder Create()
        {
            return new();
        }

        public SupplierEntityBuilder WithId(string id)
        {
            entity.Id = id;
            return this;
        }

        public SupplierEntityBuilder WithName(string name)
        {
            entity.Name = name;
            return this;
        }

        public SupplierEntityBuilder WithSummary(string summary)
        {
            entity.Summary = summary;
            return this;
        }

        public SupplierEntityBuilder WithSupplierUrl(string supplierUrl)
        {
            entity.SupplierUrl = supplierUrl;
            return this;
        }

        public SupplierEntityBuilder WithAddress(string address)
        {
            entity.Address = address;
            return this;
        }

        public SupplierEntity Build()
        {
            return entity;
        }
    }
}
