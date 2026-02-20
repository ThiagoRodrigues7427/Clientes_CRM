using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Clientes.Application.Abstracoes;
using Clientes.Domain.Entidades;
using Clientes.Domain.Enums;
using Clientes.Domain.Valores;
using MediatR;

namespace Clientes.Application.Clientes;

public sealed class CriarClienteHandler : IRequestHandler<CriarClienteCommand, Guid>
{
    private readonly IRepositorioEvento _eventStore;
    private readonly IRepositorioLeituraCliente _readRepo;
    private readonly IServicoViaCep _viaCep;

    public CriarClienteHandler(IRepositorioEvento eventStore, IRepositorioLeituraCliente readRepo, IServicoViaCep viaCep)
    {
        _eventStore = eventStore;
        _readRepo = readRepo;
        _viaCep = viaCep;
    }

    public async Task<Guid> Handle(CriarClienteCommand request, CancellationToken cancellationToken)
    {
        var documento = new Documento(request.Documento);
        var email = new Email(request.Email);
        var logradouro = request.Logradouro;
        var bairro = request.Bairro;
        var cidade = request.Cidade;
        var estado = request.Estado;
        if (string.IsNullOrWhiteSpace(logradouro) || string.IsNullOrWhiteSpace(bairro) || string.IsNullOrWhiteSpace(cidade) || string.IsNullOrWhiteSpace(estado))
        {
            var via = await _viaCep.ObterEnderecoAsync(request.Cep);
            if (via != null)
            {
                logradouro = string.IsNullOrWhiteSpace(logradouro) ? via.Logradouro : logradouro;
                bairro = string.IsNullOrWhiteSpace(bairro) ? via.Bairro : bairro;
                cidade = string.IsNullOrWhiteSpace(cidade) ? via.Localidade : cidade;
                estado = string.IsNullOrWhiteSpace(estado) ? via.Uf : estado;
            }
        }
        var endereco = new Endereco(request.Cep, logradouro, request.Numero, bairro, cidade, estado);
        var tipo = request.TipoPessoa == "Fisica" ? TipoPessoa.Fisica : TipoPessoa.Juridica;
        var cliente = Cliente.Criar(request.NomeOuRazaoSocial, documento, request.DataNascimentoOuFundacao, request.Telefone, email, endereco, tipo, request.Usuario);

        var dados = new
        {
            clienteId = cliente.Id,
            nome = cliente.NomeOuRazaoSocial,
            documento = documento.Valor,
            data = request.DataNascimentoOuFundacao,
            telefone = request.Telefone,
            email = email.Valor,
            cep = request.Cep,
            logradouro = endereco.Logradouro,
            numero = request.Numero,
            bairro = endereco.Bairro,
            cidade = endereco.Cidade,
            estado = endereco.Estado,
            tipoPessoa = tipo.ToString(),
            isentoIe = request.IsentoIe,
            ie = request.InscricaoEstadual
        };
        var json = JsonSerializer.Serialize(dados);
        await _eventStore.AppendAsync(cliente.Id, "ClienteCriado", json, request.Usuario);

        var dto = new ClienteLeituraDto
        {
            Id = cliente.Id,
            NomeOuRazaoSocial = request.NomeOuRazaoSocial,
            Documento = documento.Valor,
            TipoPessoa = tipo.ToString(),
            Telefone = request.Telefone,
            Email = email.Valor,
            Cep = request.Cep,
            Logradouro = request.Logradouro,
            Numero = request.Numero,
            Bairro = request.Bairro,
            Cidade = request.Cidade,
            Estado = request.Estado
        };
        await _readRepo.CriarOuAtualizarAsync(dto);

        return cliente.Id;
    }
}