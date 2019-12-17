using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    public class RequiredResult : IResult
    {
        public bool IsValid => !Required.Any();

        public HashSet<string> Required { get; } = new HashSet<string>();
    }
}
