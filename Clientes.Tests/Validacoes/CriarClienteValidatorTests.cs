using System;
using System.Threading.Tasks;
using Clientes.Application.Abstracoes;
using Clientes.Application.Clientes;
using FluentAssertions;
using Moq;
using Xunit;

namespace Clientes.Tests.Validacoes;

public sealed class CriarClienteValidatorTests
{
    [Fact]
    public async Task PessoaFisica_menor_de_18_deve_falhar()
    {
        var repo = new Mock<IRepositorioLeituraCliente>();
        repo.Setup(r => r.ExisteDocumentoOuEmailAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
        var validator = new CriarClienteValidator(repo.Object);
        var cmd = new CriarClienteCommand
        {
            NomeOuRazaoSocial = "João",
            Documento = "12345678901",
            DataNascimentoOuFundacao = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-17)),
            Telefone = "11999999999",
            Email = "teste@teste.com",
            Cep = "01001000",
            Logradouro = "",
            Numero = "10",
            Bairro = "",
            Cidade = "",
            Estado = "",
            TipoPessoa = "Fisica"
        };
        var res = await validator.ValidateAsync(cmd);
        res.IsValid.Should().BeFalse();
    }

    [Fact]
    public async Task PessoaJuridica_sem_IE_e_sem_isento_deve_falhar()
    {
        var repo = new Mock<IRepositorioLeituraCliente>();
        repo.Setup(r => r.ExisteDocumentoOuEmailAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(false);
        var validator = new CriarClienteValidator(repo.Object);
        var cmd = new CriarClienteCommand
        {
            NomeOuRazaoSocial = "Empresa",
            Documento = "12345678000199",
            DataNascimentoOuFundacao = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-5)),
            Telefone = "1133333333",
            Email = "contato@emp.com",
            Cep = "01001000",
            Logradouro = "Rua X",
            Numero = "100",
            Bairro = "Centro",
            Cidade = "SP",
            Estado = "SP",
            TipoPessoa = "Juridica",
            IsentoIe = false,
            InscricaoEstadual = null
        };
        var res = await validator.ValidateAsync(cmd);
        res.IsValid.Should().BeFalse();
    }
}