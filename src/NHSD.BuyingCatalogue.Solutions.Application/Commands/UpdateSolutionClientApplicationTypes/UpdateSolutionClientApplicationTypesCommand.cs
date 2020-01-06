using System.Collections.Generic;
using System.Linq;
using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionClientApplicationTypes
{
    public sealed class UpdateSolutionClientApplicationTypesCommand : IRequest<ISimpleResult>
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Updated details of a solution.
        /// </summary>
        public UpdateSolutionClientApplicationTypesViewModel Data { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionClientApplicationTypesCommand"/> class.
        /// </summary>
        public UpdateSolutionClientApplicationTypesCommand(string solutionId, UpdateSolutionClientApplicationTypesViewModel data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
            Data.ClientApplicationTypes = Data.ClientApplicationTypes == null ?
                new HashSet<string>() : 
                new HashSet<string>(Data.ClientApplicationTypes.Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => x.Trim()));
        }
    }
}
