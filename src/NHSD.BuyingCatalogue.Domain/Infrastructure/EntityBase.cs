using System.Collections.Generic;

namespace NHSD.BuyingCatalogue.Domain.Infrastructure
{
    /// <summary>
    /// Represents the common data and functionality across all entities.
    /// </summary>
    /// <typeparam name="T">Defines the type of the property <see cref="Id"/>.</typeparam>
    public abstract class EntityBase<T>
    {
        /// <summary>
        /// Unique ID of the entity.
        /// </summary>
        public T Id { get; set; }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="other">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public bool Equals(EntityBase<T> other)
        {
            return other is object
                && EqualityComparer<T>.Default.Equals(Id, other.Id);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (!(obj is EntityBase<T> item))
            {
                return false;
            }

            return Equals(item);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return EqualityComparer<T>.Default.GetHashCode(Id);
        }
    }
}
