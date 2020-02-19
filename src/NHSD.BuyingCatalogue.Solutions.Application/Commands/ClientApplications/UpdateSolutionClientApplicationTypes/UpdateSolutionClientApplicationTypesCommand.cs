using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.ClientApplications.UpdateSolutionClientApplicationTypes
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
        public IUpdateSolutionClientApplicationTypes Data { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionClientApplicationTypesCommand"/> class.
        /// </summary>
        public UpdateSolutionClientApplicationTypesCommand(string solutionId, IUpdateSolutionClientApplicationTypes data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}
