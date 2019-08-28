using MediatR;

namespace NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities
{
	public sealed class ListCapabilitiesQuery : IRequest<ListCapabilitiesQueryResult>
	{
		/// <summary>
		/// Initialises a new instance of the <see cref="ListCapabilitiesQuery"/> class.
		/// </summary>
		public ListCapabilitiesQuery()
		{
		}
	}
}
