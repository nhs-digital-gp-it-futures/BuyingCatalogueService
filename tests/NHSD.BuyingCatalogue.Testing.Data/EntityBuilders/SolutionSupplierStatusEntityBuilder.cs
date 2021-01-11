using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public sealed class SolutionSupplierStatusEntityBuilder
    {
        private readonly SolutionSupplierStatusEntity _SolutionSupplierStatusEntity;

        public static SolutionSupplierStatusEntityBuilder Create()
        {
            return new();
        }

        public SolutionSupplierStatusEntityBuilder()
        {
            //Default
            var id = 10;

            _SolutionSupplierStatusEntity = new SolutionSupplierStatusEntity
            {
                Id = id,
                Name = $"Solution Supplier Status {id}",
            };
        }

        public SolutionSupplierStatusEntityBuilder WithId(int id)
        {
            _SolutionSupplierStatusEntity.Id = id;
            return this;
        }

        public SolutionSupplierStatusEntityBuilder WithName(string name)
        {
            _SolutionSupplierStatusEntity.Name = name;
            return this;
        }

        public SolutionSupplierStatusEntity Build()
        {
            return _SolutionSupplierStatusEntity;
        }
    }
}
