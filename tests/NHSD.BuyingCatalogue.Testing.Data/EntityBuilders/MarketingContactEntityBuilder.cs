using System;
using NHSD.BuyingCatalogue.Testing.Data.Entities;

namespace NHSD.BuyingCatalogue.Testing.Data.EntityBuilders
{
    public class MarketingContactEntityBuilder
    {
        private readonly MarketingContactEntity _entity;

        public static MarketingContactEntityBuilder Create()
        {
            return new();
        }

        public MarketingContactEntityBuilder()
        {
            _entity = new MarketingContactEntity
            {
                SolutionId = "Sup 1"
            };
        }

        public MarketingContactEntityBuilder WithSolutionId(string id)
        {
            _entity.SolutionId = id;
            return this;
        }

        public MarketingContactEntityBuilder WithFirstName(string name)
        {
            _entity.FirstName = name;
            return this;
        }

        public MarketingContactEntityBuilder WithLastName(string name)
        {
            _entity.LastName = name;
            return this;
        }

        public MarketingContactEntityBuilder WithEmail(string email)
        {
            _entity.Email = email;
            return this;
        }

        public MarketingContactEntityBuilder WithPhoneNumber(string number)
        {
            _entity.PhoneNumber = number;
            return this;
        }

        public MarketingContactEntityBuilder WithDepartment(string department)
        {
            _entity.Department = department;
            return this;
        }

        public MarketingContactEntityBuilder WithLastUpdated(DateTime lastUpdated)
        {
            _entity.LastUpdated = lastUpdated;
            return this;
        }

        public MarketingContactEntityBuilder WithLastUpdatedBy(Guid lastUpdatedBy)
        {
            _entity.LastUpdatedBy = lastUpdatedBy;
            return this;
        }

        public MarketingContactEntity Build()
        {
            return _entity;
        }
    }
}
