using MediatR;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAll
{
	/// <summary>
	/// Represents the query paramters for the get all solutions request.
	/// </summary>
	public sealed class GetAllSolutionSummariesQuery : IRequest<GetAllSolutionSummariesQueryResult>
	{
		/// <summary>
		/// Initialises a new instance of the <see cref="GetAllSolutionSummariesQuery"/> class.
		/// </summary>
		public GetAllSolutionSummariesQuery()
		{
		}
	}
}
