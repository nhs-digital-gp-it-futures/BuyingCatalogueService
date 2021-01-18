using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NHSD.BuyingCatalogue.Infrastructure
{
    /// <summary>
    /// Represents an enumeration.
    /// </summary>
    public abstract class Enumerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enumerator"/> class.
        /// </summary>
        /// <param name="id">The ID of this instance.</param>
        /// <param name="name">The name of this instance.</param>
        protected Enumerator(int id, string name)
        {
            Id = id;
            Name = name;
        }

        /// <summary>
        /// Gets the ID of this instance.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Gets the name of this instance.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets all values of the specified Enumerator type.
        /// </summary>
        /// <typeparam name="T">The type of Enumerator to retrieve.</typeparam>
        /// <returns>All values of the specified Enumerator type.</returns>
        public static IEnumerable<T> GetAll<T>()
            where T : Enumerator
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            return fields.Select(f => f.GetValue(null)).Cast<T>();
        }

        /// <summary>
        /// Converts the specified value into the typed Enumerator.
        /// </summary>
        /// <typeparam name="T">Type of Enumerator.</typeparam>
        /// <param name="value">The ID value of an Enumerator.</param>
        /// <returns>The specified value converted into the typed Enumerator.</returns>
        public static T FromValue<T>(int value)
            where T : Enumerator => Parse<T, int>(value, "value", item => item.Id == value);

        /// <summary>
        /// Converts the specified <paramref name="name"/> into the typed Enumerator.
        /// </summary>
        /// <typeparam name="T">Type of Enumerator.</typeparam>
        /// <param name="name">The name value of an Enumerator.</param>
        /// <returns>The specified <paramref name="name"/> converted into the typed Enumerator..</returns>
        public static T FromName<T>(string name)
            where T : Enumerator
        {
            return Parse<T, string>(
                name,
                "name",
                item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj) =>
            obj is Enumerator comparisonValue && GetType() == comparisonValue.GetType() && Equals(Id, comparisonValue.Id);

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode() => Id.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => Name;

        /// <summary>
        /// Gets the typed Enumerator based on the supplied <paramref name="predicate"/>.
        /// </summary>
        /// <typeparam name="T">Type of Enumerator.</typeparam>
        /// <typeparam name="TV">Property type of the Enumerator.</typeparam>
        /// <param name="value">Property value.</param>
        /// <param name="description">Property description.</param>
        /// <param name="predicate">Filter to find the correctly typed Enumerator.</param>
        /// <returns>The typed Enumerator based on the supplied <paramref name="predicate"/>.</returns>
        private static T Parse<T, TV>(TV value, string description, Func<T, bool> predicate)
            where T : Enumerator
        {
            return GetAll<T>().FirstOrDefault(predicate)
                ?? throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");
        }
    }
}
