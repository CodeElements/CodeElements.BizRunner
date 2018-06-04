using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CodeElements.BizRunner
{
    /// <summary>
    ///     Base class for advanced error handling
    /// </summary>
    public abstract class BusinessActionErrors : BizActionStatus
    {
        /// <summary>
        ///     Validate the given model and add errors
        /// </summary>
        /// <param name="model">The model to be validated</param>
        /// <returns>Return <see cref="BizActionStatus.HasErrors" /></returns>
        protected virtual bool ValidateModelFailed(IValidatableObject model)
        {
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(model, new ValidationContext(model), results);

            AddValidationResults(results);
            return HasErrors;
        }

        /// <summary>
        ///     Add the given validation result and return the default value of <see cref="T" />
        /// </summary>
        /// <typeparam name="T">The parameter type</typeparam>
        /// <param name="validationResult">The error result</param>
        /// <returns>Return <code>default(T)</code></returns>
        protected virtual T ReturnError<T>(ValidationResult validationResult)
        {
            AddValidationResult(validationResult);
            return default(T);
        }

        /// <summary>
        ///     Inherit the errors of another <see cref="IBizActionStatus" />
        /// </summary>
        /// <param name="status">The status that provides the errors</param>
        /// <returns>Return <see cref="BizActionStatus.HasErrors" /></returns>
        protected virtual bool InheritStatusFailed(IBizActionStatus status)
        {
            if (status.HasErrors)
            {
                foreach (var error in status.Errors)
                    AddValidationResult(error);

                return true;
            }

            return false;
        }
    }
}