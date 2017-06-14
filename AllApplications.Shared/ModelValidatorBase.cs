using FluentValidation;
using System.Linq;

namespace Core
{
    public interface IModelValidator
    {
        NotificationItem ValidateProperty(object item, string propertyName);
    }

    public interface IModelValidator<T> : IModelValidator
    {
        Notification ValidateModel(T instance);

        NotificationItem ValidateProperty(T instance, string propertyName);
    }

    public abstract class ModelValidatorBase<T> : AbstractValidator<T>, IModelValidator<T>
    {
        public Notification ValidateModel(T instance)
        {
            Notification result = new Notification();

            if (instance != null)
            {
                var validationResult = Validate(instance);

                foreach (var error in validationResult.Errors)
                {
                    NotificationItem errorItem = new NotificationItem(error.ErrorMessage, NotificationSeverity.Error)
                    {
                        Reference = error.PropertyName
                    };
                    result.Add(errorItem);
                }
            }

            return result;
        }

        public NotificationItem ValidateProperty(object item, string propertyName)
        {
            return ValidateProperty(item, propertyName);
        }

        public NotificationItem ValidateProperty(T instance, string propertyName)
        {
            Notification validationResult = ValidateModel(instance);

            var propertyError = validationResult.Items.FirstOrDefault(x => x.Reference == propertyName);

            return propertyError;
        }
    }
}