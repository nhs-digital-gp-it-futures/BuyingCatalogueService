namespace NHSD.BuyingCatalogue.Solutions.Contracts
{
    public interface IPlugins
    {
        bool? Required { get; }

        string AdditionalInformation { get; }
    }
}
