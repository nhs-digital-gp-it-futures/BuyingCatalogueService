using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class AboutSupplierSection
    {
        public AboutSupplierSection(ISolutionSupplier solutionSupplier)
        {
            Answers = new AboutSupplierSectionAnswers(solutionSupplier);
        }

        public AboutSupplierSectionAnswers Answers { get; }

        public AboutSupplierSection IfPopulated() => Answers.HasData ? this : null;
    }
}
