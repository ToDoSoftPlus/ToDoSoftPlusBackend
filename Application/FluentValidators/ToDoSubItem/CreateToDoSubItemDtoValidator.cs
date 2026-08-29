using Application.DTOs.ToDoSubItem;
using FluentValidation;

namespace Application.FluentValidators.ToDoSubItem
{
    public class CreateToDoSubItemDtoValidator : AbstractValidator<CreateToDoSubItemDto>
    {
        public CreateToDoSubItemDtoValidator()
        {
            RuleFor(x => x.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Description is required.")
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

            RuleFor(x => x.ToDoItemId)
                .GreaterThan(0).WithMessage("ToDoItemId is required.");
        }
    }
}
