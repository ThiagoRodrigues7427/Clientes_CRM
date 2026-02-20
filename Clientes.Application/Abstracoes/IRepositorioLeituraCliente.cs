using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clientes.Application.Abstracoes;

public sealed class ClienteLeituraDto
{
    public Guid Id { get; set; }
    public string NomeOuRazaoSocial { get; set; } = string.Empty;
    public string Documento { get; set; } = string.Empty;
    public string TipoPessoa { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cep { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string Numero { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
}

public interface IRepositorioLeituraCliente
{
    Task<ClienteLeituraDto?> ObterPorIdAsync(Guid id);
    Task<IReadOnlyList<ClienteLeituraDto>> ListarAsync();
    Task CriarOuAtualizarAsync(ClienteLeituraDto dto);
    Task<bool> ExisteDocumentoOuEmailAsync(string documento, string email);
}