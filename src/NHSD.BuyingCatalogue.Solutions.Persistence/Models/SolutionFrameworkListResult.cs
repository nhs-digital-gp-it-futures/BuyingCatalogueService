using System;
using NHSD.BuyingCatalogue.Solutions.Contracts.Persistence;

namespace NHSD.BuyingCatalogue.Solutions.Persistence.Models
{
    internal sealed class SolutionFrameworkListResult : ISolutionFrameworkListResult
    {
        public string Id { get; set; }

        public string FrameworkName { get; set; }
    }
}
