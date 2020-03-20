using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionSummary
{
    public sealed class UpdateSolutionSummaryCommand : IRequest<ISimpleResult>
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Updated details of a solution.
        /// </summary>
        public IUpdateSolutionSummary Data { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionSummaryCommand"/> class.
        /// </summary>
        public UpdateSolutionSummaryCommand(string solutionId, IUpdateSolutionSummary data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}
