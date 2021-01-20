namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal sealed class MaxLengthValidator
    {
        private readonly MaxLengthResult maxLengthResult;

        internal MaxLengthValidator() => maxLengthResult = new MaxLengthResult();

        internal MaxLengthValidator Validate(string field, int limit, string fieldLabel)
        {
            if ((field?.Length ?? 0) > limit)
            {
                maxLengthResult.MaxLength.Add(fieldLabel);
            }

            return this;
        }

        internal MaxLengthResult Result() => maxLengthResult;
    }
}
