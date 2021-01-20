using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Application.Domain
{
    internal sealed class MobileOperatingSystems
    {
        public HashSet<string> OperatingSystems { get; set; } = new();

        public string OperatingSystemsDescription { get; set; }
    }
}
