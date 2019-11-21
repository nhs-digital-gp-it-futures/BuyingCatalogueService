using System;
using NHSD.BuyingCatalogue.Contracts.Capability;

namespace NHSD.BuyingCatalogue.Capabilities.API.ViewModels
{
	/// <summary>
	/// Provides the view representation for the <see cref="Capability"/> entity.
	/// </summary>
	public sealed class CapabilityViewModel
	{
        public CapabilityViewModel(ICapability capability)
        {
            Id = capability.Id;
            Name = capability.Name;
            IsFoundation = capability.IsFoundation;
        }

        /// <summary>
		/// Identifier of the capability.
		/// </summary>
		public Guid Id { get;}

		/// <summary>
		/// Name of the capability.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// A value to determine whether or not this instance is the criteria to form a foundation solution.
		/// </summary>
		public bool IsFoundation { get; }
    }
}
