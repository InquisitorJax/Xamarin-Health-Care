using Core;
using FluentValidation;

namespace SampleApplication
{
    public class SampleItemValidator : ModelValidatorBase<SampleItem>
    {
        public SampleItemValidator()
        {
            RuleFor(item => item.Name).NotEmpty().WithMessage("Please provide a value for Name");
        }
    }
}