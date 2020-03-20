using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class AboutSupplierSection
    {
        public AboutSupplierSectionAnswers Answers { get; }

        public AboutSupplierSection(ISupplier supplier)
        {
            Answers = new AboutSupplierSectionAnswers(supplier);
        }

        public AboutSupplierSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}
