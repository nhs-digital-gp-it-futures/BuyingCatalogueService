using System;

namespace NHSD.BuyingCatalogue.Application.Capabilities.Queries.ListCapabilities
{
	/// <summary>
	/// Provides the view representation for the <see cref="Capability"/> entity.
	/// </summary>
	public sealed class CapabilityViewModel
	{
		/// <summary>
		/// Identifier of the capability.
		/// </summary>
		public Guid Id { get; set; }

		/// <summary>
		/// Name of the capability.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// A value to determine whether or not this instance is the criteria to form a foundation solution.
		/// </summary>
		public bool IsFoundation { get; set; }

		/// <summary>
		/// Initialises a new instance of the <see cref="CapabilityViewModel"/> class.
		/// </summary>
		public CapabilityViewModel()
		{
		}
	}
}
