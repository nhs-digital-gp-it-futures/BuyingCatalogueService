using Newtonsoft.Json.Linq;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    public sealed class UpdateSolutionViewModel
    {
        /// <summary>
        /// Description of the solution.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Summary of the solution.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// A link to more information regarding the solution.
        /// </summary>
        public string AboutUrl { get; set; }

        /// <summary>
        /// Marketing data of the solution.
        /// </summary>
        public JObject MarketingData { get; set; }
    }
}
