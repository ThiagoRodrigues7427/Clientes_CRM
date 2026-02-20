using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clientes.Application.Abstracoes;
using Clientes.Infrastructure.Leitura;
using Microsoft.EntityFrameworkCore;

namespace Clientes.Infrastructure.Repositorios;

public sealed class RepositorioLeituraCliente : IRepositorioLeituraCliente
{
    private readonly ContextoLeitura _ctx;
    public RepositorioLeituraCliente(ContextoLeitura ctx)
    {
        _ctx = ctx;
    }
    public async Task<ClienteLeituraDto?> ObterPorIdAsync(Guid id)
    {
        var c = await _ctx.Clientes.FindAsync(id);
        if (c == null) return null;
        return Map(c);
    }
    public async Task<IReadOnlyList<ClienteLeituraDto>> ListarAsync()
    {
        return await _ctx.Clientes.AsNoTracking().Select(c => Map(c)).ToListAsync();
    }
    public async Task CriarOuAtualizarAsync(ClienteLeituraDto dto)
    {
        var existe = await _ctx.Clientes.FindAsync(dto.Id);
        if (existe == null)
        {
            var novo = new ClienteLeitura
            {
                Id = dto.Id,
                NomeOuRazaoSocial = dto.NomeOuRazaoSocial,
                Documento = dto.Documento,
                TipoPessoa = dto.TipoPessoa,
                Telefone = dto.Telefone,
                Email = dto.Email,
                Cep = dto.Cep,
                Logradouro = dto.Logradouro,
                Numero = dto.Numero,
                Bairro = dto.Bairro,
                Cidade = dto.Cidade,
                Estado = dto.Estado
            };
            _ctx.Clientes.Add(novo);
        }
        else
        {
            existe.NomeOuRazaoSocial = dto.NomeOuRazaoSocial;
            existe.Documento = dto.Documento;
            existe.TipoPessoa = dto.TipoPessoa;
            existe.Telefone = dto.Telefone;
            existe.Email = dto.Email;
            existe.Cep = dto.Cep;
            existe.Logradouro = dto.Logradouro;
            existe.Numero = dto.Numero;
            existe.Bairro = dto.Bairro;
            existe.Cidade = dto.Cidade;
            existe.Estado = dto.Estado;
        }
        await _ctx.SaveChangesAsync();
    }
    public async Task<bool> ExisteDocumentoOuEmailAsync(string documento, string email)
    {
        return await _ctx.Clientes.AsNoTracking().AnyAsync(c => c.Documento == documento || c.Email == email);
    }
    static ClienteLeituraDto Map(ClienteLeitura c)
    {
        return new ClienteLeituraDto
        {
            Id = c.Id,
            NomeOuRazaoSocial = c.NomeOuRazaoSocial,
            Documento = c.Documento,
            TipoPessoa = c.TipoPessoa,
            Telefone = c.Telefone,
            Email = c.Email,
            Cep = c.Cep,
            Logradouro = c.Logradouro,
            Numero = c.Numero,
            Bairro = c.Bairro,
            Cidade = c.Cidade,
            Estado = c.Estado
        };
    }
}