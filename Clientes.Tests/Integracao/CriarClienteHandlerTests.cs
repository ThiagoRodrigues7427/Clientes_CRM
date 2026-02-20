using System;
using System.Threading;
using System.Threading.Tasks;
using Clientes.Application.Abstracoes;
using Clientes.Application.Clientes;
using Clientes.Infrastructure.Leitura;
using Clientes.Infrastructure.Repositorios;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Clientes.Tests.Integracao;

public sealed class CriarClienteHandlerTests
{
    [Fact]
    public async Task Deve_criar_cliente_e_persistir_na_leitura()
    {
        var options = new DbContextOptionsBuilder<ContextoLeitura>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        await using var ctx = new ContextoLeitura(options);
        var readRepo = new RepositorioLeituraCliente(ctx);
        var eventRepo = new Mock<IRepositorioEvento>();
        var viaCep = new Mock<IServicoViaCep>();
        viaCep.Setup(v => v.ObterEnderecoAsync(It.IsAny<string>())).ReturnsAsync((ViaCepResposta?)null);
        var handler = new CriarClienteHandler(eventRepo.Object, readRepo, viaCep.Object);
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
            Cidade = "São Paulo",
            Estado = "SP",
            TipoPessoa = "Juridica",
            IsentoIe = true
        };
        var id = await handler.Handle(cmd, CancellationToken.None);
        id.Should().NotBe(Guid.Empty);
        var salvo = await readRepo.ObterPorIdAsync(id);
        salvo.Should().NotBeNull();
        salvo!.Email.Should().Be("contato@emp.com");
    }
}