namespace NHSD.BuyingCatalogue.Solutions.Application.Commands.Validation
{
    internal class MaxLengthValidator
    {
        private readonly MaxLengthResult _maxLengthResult;

        internal MaxLengthValidator() => _maxLengthResult = new MaxLengthResult();

        internal MaxLengthValidator Validate(string field, int limit, string fieldLabel)
        {
            if ((field?.Length ?? 0) > limit)
            {
                _maxLengthResult.MaxLength.Add(fieldLabel);
            }

            return this;
        }

        internal MaxLengthResult Result() => _maxLengthResult;
    }
}
