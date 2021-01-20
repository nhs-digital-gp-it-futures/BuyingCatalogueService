namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal interface IValidator<in T, out TResult>
        where TResult : IResult
    {
        TResult Validate(T command);
    }
}
