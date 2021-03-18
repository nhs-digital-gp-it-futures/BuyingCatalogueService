using System;

namespace NHSD.BuyingCatalogue.Solutions.Contracts.Persistence
{
    public interface ISolutionFrameworkListResult
    {
        public string Id { get; }

        public string FrameworkName { get; }
    }
}
