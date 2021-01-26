using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public class MarketingContactEntityBuilder
    {
        private readonly MarketingContactEntity entity;

        public MarketingContactEntityBuilder()
        {
            entity = new MarketingContactEntity
            {
                SolutionId = "Sup 1",
            };
        }

        public static MarketingContactEntityBuilder Create()
        {
            return new();
        }

        public MarketingContactEntityBuilder WithSolutionId(string id)
        {
            entity.SolutionId = id;
            return this;
        }

        public MarketingContactEntityBuilder WithFirstName(string name)
        {
            entity.FirstName = name;
            return this;
        }

        public MarketingContactEntityBuilder WithLastName(string name)
        {
            entity.LastName = name;
            return this;
        }

        public MarketingContactEntityBuilder WithEmail(string email)
        {
            entity.Email = email;
            return this;
        }

        public MarketingContactEntityBuilder WithPhoneNumber(string number)
        {
            entity.PhoneNumber = number;
            return this;
        }

        public MarketingContactEntityBuilder WithDepartment(string department)
        {
            entity.Department = department;
            return this;
        }

        public MarketingContactEntity Build()
        {
            return entity;
        }
    }
}
