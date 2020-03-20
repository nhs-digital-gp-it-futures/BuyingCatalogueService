namespace NHSD.BuyingCatalogue.Solutions.Contracts.Commands
{
    public interface IUpdateSolutionContact
    {
        string Department { get; }

        string Email { get; }

        string FirstName { get; }

        string LastName { get; }

        string PhoneNumber { get; }

        bool HasData();

        void Trim();
    }
}
