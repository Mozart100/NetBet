using NetBet.Infrastracture;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace NetBet.Services.Validations
{
    public abstract class ServiceValidatorBase
    {
        protected virtual IList<NetBetError> Dissect(ValidationResult validationResult)
        {
            var errors = new List<NetBetError>();
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(new NetBetError(errorMessage: error.ErrorMessage, propertyName: error.PropertyName));
                }
            }

            return errors;
        }

        protected void Validate(IEnumerable<NetBetError> errors)
        {
            if (errors.SafeAny())
            {
                var instance = new NetBetException(errors.ToArray());
                throw instance;
            }
        }
    }
}
