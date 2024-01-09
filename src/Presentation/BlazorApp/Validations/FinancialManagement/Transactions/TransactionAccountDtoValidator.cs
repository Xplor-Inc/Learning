namespace XploringMe.BlazorApp.FluentValidation.Finance.Transactions;
public class TransactionAccountDtoValidator : AbstractValidator<TransactionAccountDto>
{
    public TransactionAccountDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(m => m.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(StaticConfiguration.NAME_LENGTH)
            .WithMessage("Name is too long");

        RuleFor(m => m.AccountNumber)
            .NotEmpty()
            .WithMessage("AccountNumber is required")
            .MaximumLength(StaticConfiguration.NAME_LENGTH)
            .WithMessage("AccountNumber is too long");

        RuleFor(m => m.StatementDay)
            .GreaterThan(0)
            .WithMessage("StatementDay should be grater than 0")
            .LessThan(31)
            .WithMessage("StatementDay should be less than 31");

        When(d => !string.IsNullOrEmpty(d.DebitCardNo), () =>
        {
            RuleFor(d => d.DebitCardNo)
                .CreditCard()
                .WithMessage("Card Nnumber not valid ")
                .MaximumLength(16)
                .WithMessage("DebitCardNo should be 16 digit")
                .MinimumLength(16)
                .WithMessage("DebitCardNo should be 16 digit");

            RuleFor(d => d.DebitCardPIN)
                .ExclusiveBetween(1000,9999)
                .WithMessage("DebitCard PIN  should be 4 digit")
                .NotEmpty()
                .WithMessage("DebitCard PIN is required");
            
            RuleFor(d => d.DebitCardCVV)
                .ExclusiveBetween(100, 9999)
                .WithMessage("DebitCard CVV should be 3/4 digit")
                .NotEmpty()
                .WithMessage("DebitCard CVV is required");

            RuleFor(d => d.DebitCardExpireDate)
                .NotEmpty()
                .WithMessage("DebitCard ExpireDate is required");
        });

        When(d => !string.IsNullOrEmpty(d.UPIId), () =>
        {           
            RuleFor(d => d.UPIPIN)
                .ExclusiveBetween(1000, 999999)
                .WithMessage("UPI PIN  should be 4 to 6 digit")
                .NotEmpty()
                .WithMessage("DebitCard PIN is required");
        });

        When(d => !string.IsNullOrEmpty(d.NetBankingURL), () =>
        {
            RuleFor(d => d.NetBankingUserId)
                .NotEmpty()
                .WithMessage("NetBanking UserId is required");

            RuleFor(d => d.NetBankingPassword)
                .NotEmpty()
                .WithMessage("NetBanking Password is required");
        });
    }
}
