using System;
using System.Collections.Generic;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionContactDetails
{
    public class ContactsMaxLengthResult : IResult
    {
        public ContactsMaxLengthResult(ISimpleResult contact1Result, ISimpleResult contact2Result)
        {
            Contact1Result = contact1Result ?? throw new ArgumentNullException(nameof(contact1Result));
            Contact2Result = contact2Result ?? throw new ArgumentNullException(nameof(contact2Result));
        }

        internal ISimpleResult Contact1Result { get; }

        internal ISimpleResult Contact2Result { get; }

        public bool IsValid => Contact1Result.IsValid && Contact2Result.IsValid;

        public Dictionary<string, Dictionary<string, string>> ToDictionary() =>
            new Dictionary<string, Dictionary<string, string>>
            {
                {"contact-1", Contact1Result.IsValid ? null : Contact1Result.ToDictionary()},
                {"contact-2", Contact2Result.IsValid ? null : Contact2Result.ToDictionary()},
            }
            .FilterNulls();
    }
}
