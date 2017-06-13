using Core;
using FluentValidation;
using SampleApplication.Models;

namespace SampleApplication
{
    public class AppointmentValidator : ModelValidatorBase<Appointment>
    {
        public AppointmentValidator()
        {
            RuleFor(item => item.Name).NotEmpty().WithMessage("Please provide a value for Name");
            RuleFor(item => item.AppointmentDate).NotEmpty().WithMessage("Please provide a value for Appointment Date");
        }
    }
}