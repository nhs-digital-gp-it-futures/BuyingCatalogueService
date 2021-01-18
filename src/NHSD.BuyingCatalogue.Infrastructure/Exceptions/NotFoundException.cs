using System;
using System.Runtime.Serialization;

namespace NHSD.BuyingCatalogue.Infrastructure.Exceptions
{
    [Serializable]
    public sealed class NotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotFoundException"/> class.
        /// </summary>
        /// <param name="name"> The name of the entity that could not be found.</param>
        /// <param name="key">The key of the entity that could not be found.</param>
        public NotFoundException(string name, object key)
            : base($"Entity named '{name}' could not be found matching the ID '{key}'.")
        {
        }

        public NotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotFoundException(string message)
            : base(message)
        {
        }

        public NotFoundException()
        {
        }

        private NotFoundException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}
