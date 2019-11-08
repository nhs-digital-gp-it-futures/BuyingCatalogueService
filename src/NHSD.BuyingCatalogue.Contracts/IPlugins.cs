namespace NHSD.BuyingCatalogue.Contracts
{
    public interface IPlugins
    {
        bool? Required { get; }

        string AdditionalInformation { get; }
    }
}
