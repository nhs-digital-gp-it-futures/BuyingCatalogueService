using NHSD.BuyingCatalogue.Solutions.Contracts;

namespace NHSD.BuyingCatalogue.Solutions.API.QueryModels
{
    public sealed class SupplierSearchQuery
    {
        private PublishedStatus? _solutionPublicationStatus;

        public string Name { get; set; }

        public PublishedStatus? SolutionPublicationStatus
        {
            get
            {
                return LimitToPublishedSolutions.GetValueOrDefault()
                    ? PublishedStatus.Published
                    : _solutionPublicationStatus;
            }
            set
            {
                _solutionPublicationStatus = value;
            }
        }

        public bool? LimitToPublishedSolutions { get; set; }
    }
}
