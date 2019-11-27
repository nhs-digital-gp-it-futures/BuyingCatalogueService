using System;

namespace NHSD.BuyingCatalogue.Infrastructure.Exceptions
{
    public sealed class NotFoundException : Exception
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        public NotFoundException(string name, object key) : base($"Entity named '{name}' could not be found matching the ID '{key}'.")
        {
        }
    }
}
