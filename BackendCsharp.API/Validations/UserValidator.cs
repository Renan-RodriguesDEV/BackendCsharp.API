using BackendCsharp.API.DTOs;
using FluentValidation;

namespace BackendCsharp.API.Validations
{
    public class UserValidator:AbstractValidator<UserRequests>
    {
        public UserValidator()
        {
            RuleFor(user => user.Username).NotEmpty().WithMessage("Username dont empty");
            RuleFor(user => user.Password).NotEmpty().WithMessage("Password dont empty");
        }
    }
}
