namespace XploringMe.BlazorApp.FluentValidation.Enquiries;

public class EnquiryResolutionDtoValidator : AbstractValidator<EnquiryResolutionDto>
{
    public EnquiryResolutionDtoValidator()
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(m => m.Resolution)
           .NotEmpty()
           .WithMessage("Resolution is required")
           .MaximumLength(StaticConfiguration.COMMAN_LENGTH)
           .WithMessage("Resolution is too long");
    }
}