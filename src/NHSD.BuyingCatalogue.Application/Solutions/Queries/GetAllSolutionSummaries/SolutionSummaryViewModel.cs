using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Application.Solutions.Queries.GetAll
{
	/// <summary>
	/// Represents the data to summarize a <see cref="Solution"/> entity and associated relationships.
	/// </summary>
	public sealed class SolutionSummaryViewModel
	{
		/// <summary>
		/// Identifier of the solution.
		/// </summary>
		public string Id { get; set; }

		/// <summary>
		/// Name of the solution.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Summary of the solution.
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		/// Details of the organisation associated with the solution.
		/// </summary>
		public SolutionOrganisationViewModel Organisation { get; set; }

		/// <summary>
		/// List of capabilities linked with the solution.
		/// </summary>
		public IEnumerable<SolutionCapabilityViewModel> Capabilities { get; set; }
	}
}