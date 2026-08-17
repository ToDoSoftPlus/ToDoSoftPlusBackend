using Application.DTOs.ToDoItem;
using FluentValidation;

namespace Application.FluentValidators.ToDoItem
{
    public class UpdateToDoItemDtoValidator : AbstractValidator<UpdateToDoItemDto>
    {
        public UpdateToDoItemDtoValidator()
        {
            RuleFor(x => x.Title)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000);

            RuleFor(x => x.CompletedAt)
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CompletedAt cannot be in the future.");

            RuleFor(x => x.ToDoListId)
                .GreaterThan(0).WithMessage("ToDoListId is required.");
        }
    }
}
