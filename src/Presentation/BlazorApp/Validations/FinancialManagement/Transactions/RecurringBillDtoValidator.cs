namespace XploringMe.BlazorApp.FluentValidation.Finance.Categories;
public class RecurringBillDtoValidator : AbstractValidator<RecurringBillDto>
{
    public RecurringBillDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(m => m.StartDate.Year)
            .GreaterThan(2023)
            .WithMessage("StartDate should be start from 2024");

        RuleFor(m => m.AccountNo)
            .NotEmpty()
            .WithMessage("AccountNo is required")
            .MaximumLength(StaticConfiguration.NAME_LENGTH)
            .WithMessage("AccountNo is too long");

        RuleFor(m => m.BillName)
            .NotEmpty()
            .WithMessage("Billing Name is required")
            .MaximumLength(StaticConfiguration.NAME_LENGTH)
            .WithMessage("Billing Name is too long");

        RuleFor(m => m.Frequency)
            .IsInEnum()
            .WithMessage("Frequency is required");

        RuleFor(m => m.Amount)
            .GreaterThan(0)
            .WithMessage("Amount is required");

        When(d => d.AutoDebit, () =>
        {
            RuleFor(d => d.DebitAccountId)
                        .NotEmpty()
                        .WithMessage("DebitAccount is required");
        });
    }
}
