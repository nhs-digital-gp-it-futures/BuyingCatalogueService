using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Application.Solutions.Commands.UpdateSolution
{
    public sealed class UpdateSolutionClientApplicationTypesViewModel
    {
        /// <summary>
        /// Description of the solution.
        /// </summary>
        [JsonProperty("browser-based")]
        public bool BrowserBased { get; set; }

        /// <summary>
        /// Summary of the solution.
        /// </summary>
        [JsonProperty("native-mobile")]
        public string NativeMobile { get; set; }

        /// <summary>
        /// A link to more information regarding the solution.
        /// </summary>
        [JsonProperty("native-desktop")]
        public string NativeDesktop { get; set; }
    }
}
