namespace XploringMe.BlazorApp.FluentValidation.Finance.Categories;
public class CategoryDtoValidator : AbstractValidator<CategoryDto>
{
    public CategoryDtoValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(m => m.Type)
            .IsInEnum()
            .WithMessage("Type is required");

        RuleFor(m => m.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(StaticConfiguration.NAME_LENGTH)
            .WithMessage("Name is too long");

        RuleFor(m => m.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(StaticConfiguration.COMMAN_LENGTH)
            .WithMessage("Description is too long");

        RuleFor(m => m.Color)
            .NotEmpty()
            .WithMessage("Color is required")
            .MaximumLength(StaticConfiguration.COLOR_HEX_LENGTH)
            .WithMessage("Color is too long");
    }
}
