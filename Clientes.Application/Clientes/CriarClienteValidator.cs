using System;
using Clientes.Application.Abstracoes;
using FluentValidation;

namespace Clientes.Application.Clientes;

public sealed class CriarClienteValidator : AbstractValidator<CriarClienteCommand>
{
    public CriarClienteValidator(IRepositorioLeituraCliente repo)
    {
        RuleFor(x => x.NomeOuRazaoSocial).NotEmpty();
        RuleFor(x => x.Documento).NotEmpty();
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Telefone).NotEmpty();
        RuleFor(x => x.Cep).NotEmpty();
        RuleFor(x => x.Logradouro).NotEmpty();
        RuleFor(x => x.Numero).NotEmpty();
        RuleFor(x => x.Bairro).NotEmpty();
        RuleFor(x => x.Cidade).NotEmpty();
        RuleFor(x => x.Estado).NotEmpty();
        RuleFor(x => x.TipoPessoa).NotEmpty().Must(t => t == "Fisica" || t == "Juridica");
        RuleFor(x => x)
            .Must(x => x.TipoPessoa == "Fisica" ? CalcularIdade(x.DataNascimentoOuFundacao) >= 18 : true)
            .WithMessage("Pessoa Física deve ter pelo menos 18 anos");
        RuleFor(x => x)
            .Must(x => x.TipoPessoa == "Juridica" ? (x.IsentoIe || !string.IsNullOrWhiteSpace(x.InscricaoEstadual)) : true)
            .WithMessage("Pessoa Jurídica deve informar IE ou marcar Isento");
        RuleFor(x => x)
            .MustAsync(async (x, ct) => !await repo.ExisteDocumentoOuEmailAsync(Numeros(x.Documento), x.Email))
            .WithMessage("Documento ou email já utilizado");
    }

    static int CalcularIdade(DateOnly data)
    {
        var hoje = DateOnly.FromDateTime(DateTime.UtcNow);
        var idade = hoje.Year - data.Year;
        if (data > hoje.AddYears(-idade)) idade--;
        return idade;
    }

    static string Numeros(string valor)
    {
        return System.Text.RegularExpressions.Regex.Replace(valor ?? string.Empty, "[^0-9]", "");
    }
}