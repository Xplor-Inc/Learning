﻿namespace XploringMe.BlazorApp.Models.Dtos.Enquiries;
public class EnquiryDtoValidator : AbstractValidator<EnquiryDto>
{
    public EnquiryDtoValidator()
    {
        ValidatorOptions.Global.DefaultClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(m => m.Email)
           .NotEmpty()
           .WithMessage("Email address is required")
           .EmailAddress()
           .WithMessage("Invalid email address")
           .MaximumLength(StaticConfiguration.EMAIL_LENGTH)
           .WithMessage("Email address is too long");

        RuleFor(m => m.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(StaticConfiguration.NAME_LENGTH)
            .WithMessage("Name is too long");

        RuleFor(m => m.Message)
            .NotEmpty()
            .WithMessage("Message is required")
            .MaximumLength(StaticConfiguration.MAX_LENGTH)
            .WithMessage("Message is too long");
    }
}