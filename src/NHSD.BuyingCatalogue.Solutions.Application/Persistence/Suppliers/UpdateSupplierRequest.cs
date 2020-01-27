using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Application.Persistence.Suppliers
{
    internal sealed class UpdateSupplierRequest : IUpdateSupplierRequest
    {
        public string SolutionId { get; }
        public string Description { get; }
        public string Link { get; }

        public UpdateSupplierRequest(string solutionId, string description, string link)
        {
            SolutionId = solutionId;
            Description = description;
            Link = link;
        }
    }
}
