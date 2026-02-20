using System;
using Clientes.Domain.Enums;
using Clientes.Domain.Valores;

namespace Clientes.Domain.Eventos;

public sealed class ClienteCriadoEvento
{
    public Guid ClienteId { get; }
    public string NomeOuRazaoSocial { get; }
    public Documento Documento { get; }
    public DateOnly DataNascimentoOuFundacao { get; }
    public string Telefone { get; }
    public Email Email { get; }
    public Endereco Endereco { get; }
    public TipoPessoa TipoPessoa { get; }
    public DateTime DataEvento { get; } = DateTime.UtcNow;
    public string Usuario { get; }

    public ClienteCriadoEvento(Guid clienteId, string nomeOuRazaoSocial, Documento documento, DateOnly data, string telefone, Email email, Endereco endereco, TipoPessoa tipoPessoa, string usuario)
    {
        ClienteId = clienteId;
        NomeOuRazaoSocial = nomeOuRazaoSocial;
        Documento = documento;
        DataNascimentoOuFundacao = data;
        Telefone = telefone;
        Email = email;
        Endereco = endereco;
        TipoPessoa = tipoPessoa;
        Usuario = usuario;
    }
}