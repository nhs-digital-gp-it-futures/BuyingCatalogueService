using NHSD.BuyingCatalogue.Domain.Infrastructure;

namespace NHSD.BuyingCatalogue.Domain.Entities.Organisations
{
    /// <summary>
    /// Represents the details of a company.
    /// </summary>
    public class Organisation : EntityBase<string>
    {
        /// <summary>
        /// Name of the entity.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Summary of the entity.
        /// </summary>
        public string Summary { get; set; }

        /// <summary>
        /// Initialises a new instance of the <see cref="Organisation"/> class.
        /// </summary>
        public Organisation()
        {
        }
    }
}
