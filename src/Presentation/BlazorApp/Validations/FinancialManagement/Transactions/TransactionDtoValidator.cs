namespace XploringMe.BlazorApp.FluentValidation.Finance.Transactions;
public class TransactionDtoValidator : AbstractValidator<TransactionDto>
{
    public TransactionDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(m => m.Particular)
            .NotEmpty()
            .WithMessage("Please enter Remark")
            .MaximumLength(StaticConfiguration.COMMAN_LENGTH)
            .WithMessage($"Remark is too long, max allowed charactors is {StaticConfiguration.COMMAN_LENGTH}");

        RuleFor(m => m.CategoryId)
            .NotEmpty()
            .WithMessage("Please Select Category");

        RuleFor(m => m.TransactionType)
            .IsInEnum()
            .WithMessage("Please Select Transaction Type");


        RuleFor(m => m.TransactionAccountId)
            .GreaterThan(0)
            .WithMessage("Please Select Transaction Account");

        RuleFor(m => m.TransactionDate.Year)
            .GreaterThan(2023)
            .WithMessage("Please Select TransactionDate");

        When(m => m.TransactionType == TransactionType.Income || (m.TransactionType == TransactionType.Transfer && m.CreditAccountId.HasValue), () =>
        {
            RuleFor(m => m.Credit)
                .GreaterThan(0.01M)
                .WithMessage("Please enter Credit amount");
        });

        When(m => m.TransactionType == TransactionType.Expense || (m.TransactionType == TransactionType.Transfer && !m.CreditAccountId.HasValue), () =>
        {
            RuleFor(m => m.Debit)
                .GreaterThan(0.01M)
                .WithMessage("Please enter Debit amount");
        });

    }
}
