using Creche.Application.DTOs;
using FluentValidation;

namespace Creche.Application.Validators;

public class UnitDtoValidator : AbstractValidator<UnitDto>
{
    public UnitDtoValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("O nome é obrigatório.");
        RuleFor(x => x.Phone).NotEmpty().WithMessage("O telefone é obrigatório.")
                                 .Matches(@"^\+[1-9]\d{1,14}$").WithMessage("O telefone deve ser um número válido com código do país.");
        RuleFor(x => x.Email).NotEmpty().WithMessage("O email é obrigatório.")
                             .EmailAddress().WithMessage("O email informado não é válido.");
        RuleFor(x => x.Director).NotEmpty().WithMessage("O nome do diretor é obrigatório.");
    }
}