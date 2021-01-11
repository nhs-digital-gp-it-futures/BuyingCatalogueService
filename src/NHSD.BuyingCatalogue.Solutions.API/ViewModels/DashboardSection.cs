using Newtonsoft.Json;

namespace NHSD.BuyingCatalogue.Solutions.API.ViewModels
{
    public class DashboardSection
    {
        private readonly bool _isRequired;
        private readonly bool _isComplete;

        public string Requirement => _isRequired ? "Mandatory" : "Optional";

        public string Status => _isComplete ? "COMPLETE" : "INCOMPLETE";

        [JsonProperty("sections")]
        public object Section { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="DashboardSection"/> class.
        /// </summary>
        public DashboardSection(bool isRequired, bool isComplete, object section = null)
        {
            _isRequired = isRequired;
            _isComplete = isComplete;
            Section = section;
        }

        public static DashboardSection Mandatory(bool isComplete) => new(true, isComplete);

        public static DashboardSection Optional(bool isComplete) => new(false, isComplete);

        public static DashboardSection MandatoryWithSubSection(bool isComplete, object subSection) => new(true, isComplete, subSection);
    }
}
