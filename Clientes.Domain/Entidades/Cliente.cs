using System;
using Clientes.Domain.Core;
using Clientes.Domain.Enums;
using Clientes.Domain.Eventos;
using Clientes.Domain.Valores;

namespace Clientes.Domain.Entidades;

public sealed class Cliente : EntidadeBase
{
    public string NomeOuRazaoSocial { get; private set; }
    public Documento Documento { get; private set; }
    public DateOnly DataNascimentoOuFundacao { get; private set; }
    public string Telefone { get; private set; }
    public Email Email { get; private set; }
    public Endereco Endereco { get; private set; }
    public TipoPessoa TipoPessoa { get; private set; }

    private Cliente() {}

    public static Cliente Criar(string nomeOuRazaoSocial, Documento documento, DateOnly data, string telefone, Email email, Endereco endereco, TipoPessoa tipoPessoa, string usuario)
    {
        var cliente = new Cliente
        {
            NomeOuRazaoSocial = nomeOuRazaoSocial,
            Documento = documento,
            DataNascimentoOuFundacao = data,
            Telefone = telefone,
            Email = email,
            Endereco = endereco,
            TipoPessoa = tipoPessoa
        };
        cliente.AdicionarEvento(new ClienteCriadoEvento(cliente.Id, nomeOuRazaoSocial, documento, data, telefone, email, endereco, tipoPessoa, usuario));
        return cliente;
    }
}