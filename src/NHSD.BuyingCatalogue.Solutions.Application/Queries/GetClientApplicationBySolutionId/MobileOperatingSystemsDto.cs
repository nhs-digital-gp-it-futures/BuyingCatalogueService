using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetClientApplicationBySolutionId
{
    public sealed class MobileOperatingSystemsDto : IMobileOperatingSystems
    {
        public HashSet<string> OperatingSystems { get; internal set; } = new();

        public string OperatingSystemsDescription { get; set; }
    }
}
