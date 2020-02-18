using System;
using NHSD.BuyingCatalogue.SolutionLists.Contracts;

namespace NHSD.BuyingCatalogue.SolutionLists.API.ViewModels
{
    public sealed class CapabilityReferenceViewModel : ICapabilityReference
    {
        public string Reference { get; set; }

        private bool Equals(CapabilityReferenceViewModel other)
        {
            return Reference == other.Reference;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is CapabilityReferenceViewModel other && Equals(other);
        }

        public override int GetHashCode()
        { 
            return HashCode.Combine(Reference);
        }
    }
}
