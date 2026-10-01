using FluentValidation;
namespace Mechanicshop.Application.Features.Customers.Commands.CreateCustomer;
public sealed class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
{
    public CreateVehicleCommandValidator()
    {
        RuleFor(v => v.Make)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(v => v.Model)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(v => v.LicensePlate)
            .NotEmpty()
            .MaximumLength(10);
    }
}