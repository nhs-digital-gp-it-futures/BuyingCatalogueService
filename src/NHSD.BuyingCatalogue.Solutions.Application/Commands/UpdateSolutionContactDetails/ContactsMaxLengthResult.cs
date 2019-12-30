using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    public class ContactsMaxLengthResult : IResult
    {
        public ContactsMaxLengthResult(MaxLengthResult contact1Result, MaxLengthResult contact2Result)
        {
            Contact1Result = contact1Result.ThrowIfNull();
            Contact2Result = contact2Result.ThrowIfNull();
        }

        public MaxLengthResult Contact1Result { get; }

        public MaxLengthResult Contact2Result { get; }

        public bool IsValid => Contact1Result.IsValid && Contact2Result.IsValid;
    }
}
