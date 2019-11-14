using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolutionBrowsersSupported
{
    public sealed class UpdateSolutionBrowsersSupportedCommand : IRequest<UpdateSolutionBrowserSupportedValidationResult>
    {
        /// <summary>
        /// A value to uniquely identify a solution.
        /// </summary>
        public string SolutionId { get; }

        /// <summary>
        /// Updated details of a solution.
        /// </summary>
        public UpdateSolutionBrowsersSupportedViewModel UpdateSolutionBrowsersSupportedViewModel { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateSolutionBrowsersSupportedCommand"/> class.
        /// </summary>
        public UpdateSolutionBrowsersSupportedCommand(string solutionId, UpdateSolutionBrowsersSupportedViewModel updateSolutionBrowsersSupportedViewModel)
        {
            SolutionId = solutionId ?? throw new System.ArgumentNullException(nameof(solutionId));
            UpdateSolutionBrowsersSupportedViewModel = updateSolutionBrowsersSupportedViewModel ?? throw new System.ArgumentNullException(nameof(updateSolutionBrowsersSupportedViewModel));
        }
    }
}
