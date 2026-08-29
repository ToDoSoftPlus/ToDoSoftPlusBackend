using Application.DTOs.ToDoList;
using FluentValidation;

namespace Application.FluentValidators.ToDoList
{
    public class UpdateToDoListDtoValidator : AbstractValidator<UpdateToDoListDto>
    {
        public UpdateToDoListDtoValidator()
        {
            RuleFor(x => x.Title)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
