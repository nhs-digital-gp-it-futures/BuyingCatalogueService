using NHSD.BuyingCatalogue.Domain.Common;

namespace NHSD.BuyingCatalogue.Domain.Entities
{
	/// <summary>
	/// Represents a competencies for a ‘<see cref="Solution"/>’ can perform or provide eg
	/// * Mobile working
	/// * Training
	/// * Prescribing
	/// * Installation
	/// 
	/// Note that a ‘capability’ has a link to zero or one previous ‘capability’
	/// Generally, only interested in current ‘capability’
	/// </summary>
	public class Capability : EntityBase<string>
	{
		/// <summary>
		/// Name of the capability.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Description of the capability.
		/// </summary>
		public string Description { get; set; }
	}
}
