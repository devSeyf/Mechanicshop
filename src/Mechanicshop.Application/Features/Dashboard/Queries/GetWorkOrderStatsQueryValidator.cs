using FluentValidation;

namespace Mechanicshop.Application.Features.Dashboard.Queries;

internal class GetWorkOrderStatsQueryValidator : AbstractValidator<GetWorkOrderStatsQuery>
{
    public GetWorkOrderStatsQueryValidator()
    {
        RuleFor(request => request.Date)
            .NotEmpty()
            .WithErrorCode("Date_Is_Required")
            .WithMessage("Date is required.");
    }
}
