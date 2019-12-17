namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal interface IValidator<T, TResult> where TResult : IResult
    {
        TResult Validate(T command);
    }
}
