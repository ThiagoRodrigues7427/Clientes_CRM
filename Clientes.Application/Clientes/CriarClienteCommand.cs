using System;
using MediatR;

namespace Clientes.Application.Clientes;

public sealed class CriarClienteCommand : IRequest<Guid>
{
    public string NomeOuRazaoSocial { get; init; } = string.Empty;
    public string Documento { get; init; } = string.Empty;
    public DateOnly DataNascimentoOuFundacao { get; init; }
    public string Telefone { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Cep { get; init; } = string.Empty;
    public string Logradouro { get; init; } = string.Empty;
    public string Numero { get; init; } = string.Empty;
    public string Bairro { get; init; } = string.Empty;
    public string Cidade { get; init; } = string.Empty;
    public string Estado { get; init; } = string.Empty;
    public string TipoPessoa { get; init; } = string.Empty;
    public bool IsentoIe { get; init; }
    public string? InscricaoEstadual { get; init; }
    public string Usuario { get; init; } = "sistema";
}