using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    public interface ISimpleResult : IResult
    {
        Dictionary<string, string> ToDictionary();
    }
}
