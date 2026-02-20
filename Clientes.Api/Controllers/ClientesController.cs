using System;
using System.Threading.Tasks;
using Clientes.Application.Clientes;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Clientes.Application.Abstracoes;

namespace Clientes.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class ClientesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IRepositorioEvento _eventos;
    public ClientesController(IMediator mediator, IRepositorioEvento eventos)
    {
        _mediator = mediator;
        _eventos = eventos;
    }

    [HttpPost]
    public async Task<IActionResult> Criar([FromBody] CriarClienteCommand cmd)
    {
        var id = await _mediator.Send(cmd);
        return CreatedAtAction(nameof(ObterPorId), new { id }, new { id });
    }

    [HttpGet]
    public async Task<IActionResult> Listar()
    {
        var res = await _mediator.Send(new ListarClientesQuery());
        return Ok(res);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        var res = await _mediator.Send(new ObterClientePorIdQuery { Id = id });
        if (res == null) return NotFound();
        return Ok(res);
    }

    [HttpGet("{id:guid}/eventos")]
    public async Task<IActionResult> ListarEventos(Guid id)
    {
        var eventos = await _eventos.ListarPorAggregateAsync(id);
        return Ok(eventos);
    }
}