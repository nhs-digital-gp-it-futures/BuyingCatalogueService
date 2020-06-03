using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.QueryModels
{
    public sealed class SupplierSearchQuery
    {
        private string _solutionPublicationStatus;

        public string Name { get; set; }

        public string SolutionPublicationStatus
        {
            get
            {
                return LimitToPublishedSolutions.GetValueOrDefault()
                    ? PublishedStatus.Published.ToString()
                    : _solutionPublicationStatus?.Trim();
            }
            set
            {
                _solutionPublicationStatus = value;
            }
        }

        public bool? LimitToPublishedSolutions { get; set; }
    }
}
