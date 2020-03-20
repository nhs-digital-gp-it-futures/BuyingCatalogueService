using MediatR;
using NHSD.BuyingCatalogue.Infrastructure;
using NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation;
using NHSD.BuyingCatalogue.Solutions.Contracts.Commands;

namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.UpdateSolutionFeatures
{
    public sealed class UpdateSolutionFeaturesCommand : IRequest<ISimpleResult>
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Updated details of a solution.
        /// </summary>
        public IUpdateSolutionFeatures Data { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionFeaturesCommand"/> class.
        /// </summary>
        public UpdateSolutionFeaturesCommand(string solutionId, IUpdateSolutionFeatures data)
        {
            SolutionId = solutionId.ThrowIfNull();
            Data = data.ThrowIfNull();
        }
    }
}
