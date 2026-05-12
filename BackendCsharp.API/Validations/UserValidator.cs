using BackendCsharp.API.DTOs;
using FluentValidation;

namespace BackendCsharp.API.Validations
{
    public class UserValidator:AbstractValidator<UserRequests>
    {
        public UserValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty().WithMessage("E-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Senha é obrigatória.")
                .MinimumLength(8).WithMessage("Senha deve ter pelo menos 8 caracteres.");
        }
    }
}
