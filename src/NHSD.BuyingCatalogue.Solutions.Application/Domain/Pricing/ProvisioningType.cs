using NHSD.BuyingCatalogue.Infrastructure;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain.Pricing
{
    public sealed class ProvisioningType : Enumerator
    {
        public static readonly ProvisioningType PatientNumbers = new ProvisioningType(1, nameof(PatientNumbers));
        public static readonly ProvisioningType Declarative = new ProvisioningType(2, nameof(Declarative));
        public static readonly ProvisioningType OnDemand = new ProvisioningType(3, nameof(OnDemand));

        public ProvisioningType(int id, string name) : base(id, name)
        {
        }
    }
}
