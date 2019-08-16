using NHSD.BuyingCatalogue.Domain.Common;

namespace NHSD.BuyingCatalogue.Domain.Entities
{
	/// <summary>
	/// Represents the details of a company.
	/// </summary>
	public class Organisation : EntityBase<string>
	{
		/// <summary>
		/// Name of the entity.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Summary of the entity.
		/// </summary>
		public string Summary { get; set; }
	}
}
