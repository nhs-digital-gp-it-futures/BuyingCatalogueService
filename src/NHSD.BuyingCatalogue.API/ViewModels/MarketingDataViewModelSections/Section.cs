using System.Collections.Generic;
using System.Linq;

namespace NHSD.BuyingCatalogue.API.ViewModels
{
    public abstract class Section
    {
        protected bool _isComplete = false;

        public abstract string Id { get; }

        public string Requirement => (Mandatory.Any() || Sections != null) ? "Mandatory" : "Optional";

        public string Status => _isComplete ? "COMPLETE" : "INCOMPLETE";

        public List<string> Mandatory = new List<string>();

        public List<Section> Sections { get; protected set; } //Subsections - may be null;

    }
}
