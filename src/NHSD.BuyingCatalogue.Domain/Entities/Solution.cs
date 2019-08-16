using System.Collections.Generic;
using NHSD.BuyingCatalogue.Domain.Common;
using NHSD.BuyingCatalogue.Domain.Entities;

namespace NHSD.BuyingCatalogue.Domain
{
	/// <summary>
	/// A product and/or service provided by an ‘organisation’.
	/// </summary>
	public class Solution : EntityBase<string>
	{
		private HashSet<Capability> _capabilities = new HashSet<Capability>();

		/// <summary>
		/// Name of the solution, as displayed to a user.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Summary of the solution, as displayed to a user.
		/// </summary>
		public string Summary { get; set; }

		/// <summary>
		/// Associated organisation.
		/// </summary>
		public Organisation Organisation { get; set; }

		/// <summary>
		/// A list of capabilities associated with the solution.
		/// </summary>
		public IReadOnlyCollection<Capability> Capabilities
		{
			get
			{
				return _capabilities;
			}
		}

		/// <summary>
		/// Initialises a new instance of the <see cref="Solution"/> class.
		/// </summary>
		public Solution()
		{
		}

		/// <summary>
		/// Adds the specified capability.
		/// </summary>
		/// <param name="capability">The details of a <see cref="Capability"/>.</param>
		public void AddCapability(Capability capability)
		{
			if (capability is null)
			{
				throw new System.ArgumentNullException(nameof(capability));
			}

			_capabilities.Add(capability);
		}
	}
}
