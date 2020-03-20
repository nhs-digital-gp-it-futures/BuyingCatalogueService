using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels.Solution.Capabilities
{
    public sealed class ClaimedEpicAnswer
    {
        public string Id { get; }
        public string Name { get; }

        public ClaimedEpicAnswer(IClaimedCapabilityEpic claimedCapabilityEpic)
        {
            Id = claimedCapabilityEpic?.EpicId;
            Name = claimedCapabilityEpic?.EpicName;
        }
    }
}
