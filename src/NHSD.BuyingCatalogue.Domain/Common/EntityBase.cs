namespace NHSD.BuyingCatalogue.Domain.Common
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
	}
}
