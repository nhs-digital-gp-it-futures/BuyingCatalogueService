using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NHSD.BuyingCatalogue.Domain.Infrastructure
{
    /// <summary>
    /// Represents an enumeration.
    /// </summary>
    public abstract class Enumeration : IComparable
    {
        /// <summary>
        /// ID of this instance.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Name of this instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Enumeration"/> class.
        /// </summary>
        protected Enumeration(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Gets all values of the specified enumeration type.
        /// </summary>
        /// <typeparam name="T">The type of enumeration to retrieve.</typeparam>
        /// <returns>All values of the specified enumeration type.</returns>
        public static IEnumerable<T> GetAll<T>() where T : Enumeration
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        /// <summary>
        /// Converts the specified value into the typed enumeration.
        /// </summary>
        /// <typeparam name="T">Type of enumeration.</typeparam>
        /// <param name="value">The ID value of an enumeration.</param>
        /// <returns>The specified value converted into the typed enumeration.</returns>
        public static T FromValue<T>(int value) where T : Enumeration
        {
            return Parse<T, int>(value, "value", item => item.Id == value);
        }

        /// <summary>
        /// Converts the specified <paramref name="name"/> into the typed enumeration.
        /// </summary>
        /// <typeparam name="T">Type of enumeration.</typeparam>
        /// <param name="name">The name value of an enumeration.</param>
        /// <returns>The specified <paramref name="name"/> converted into the typed enumeration..</returns>
        public static T FromName<T>(string name) where T : Enumeration
        {
            return Parse<T, string>(name, "name", item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Gets the typed enumeration based on the supplied <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">Type of enumeration.</typeparam>
        /// <typeparam name="V">Property type of the enumeration.</typeparam>
        /// <param name="value">Property value.</param>
        /// <param name="description">Property description.</param>
        /// <param name="predicate">Filter to find the correctly typed enumeration.</param>
        /// <returns>The typed enumeration based on the supplied <paramref name="predicate"/>.</returns>
        private static T Parse<T, V>(V value, string description, Func<T, bool> predicate) where T : Enumeration
        {
            T matchingItem = GetAll<T>().FirstOrDefault(predicate);

            return matchingItem ?? throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
        }

        /// <inheritdoc/>
        public int CompareTo(object other)
        {
            return Id.CompareTo(((Enumeration)other).Id);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="comparisonValue">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        private bool Equals(Enumeration comparisonValue)
        {
            return comparisonValue is object
                && GetType() == comparisonValue.GetType()
                && Equals(Id, comparisonValue.Id);
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

            if (!(obj is Enumeration comparisonValue))
            {
                return false;
            }

            return Equals(comparisonValue);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return Name;
        }
    }
}
