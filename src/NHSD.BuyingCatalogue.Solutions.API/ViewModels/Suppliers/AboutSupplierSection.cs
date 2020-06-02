using NHSD.BuyingCatalogue.Solutions.Contracts.Suppliers;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Suppliers
{
    public sealed class AboutSupplierSection
    {
        public AboutSupplierSectionAnswers Answers { get; }

        public AboutSupplierSection(ISolutionSupplier solutionSupplier)
        {
            Answers = new AboutSupplierSectionAnswers(solutionSupplier);
        }

        public AboutSupplierSection IfPopulated()
        {
            return Answers.HasData ? this : null;
        }
    }
}
