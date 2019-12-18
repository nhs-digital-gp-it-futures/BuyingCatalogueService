using System.Collections.Generic;
using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.Application.Queries.GetSolutionById
{
    public class MobileOperatingSystemsDto : IMobileOperatingSystems
    {
        public HashSet<string> OperatingSystems { get; internal set; } = new HashSet<string>();
        public string OperatingSystemsDescription { get; set; }
    }
}
