using System;
using Clientes.Application.Abstracoes;
using MediatR;

namespace Clientes.Application.Clientes;

public sealed class ObterClientePorIdQuery : IRequest<ClienteLeituraDto?>
{
    public Guid Id { get; init; }
}

public sealed class ObterClientePorIdHandler : IRequestHandler<ObterClientePorIdQuery, ClienteLeituraDto?>
{
    private readonly IRepositorioLeituraCliente _repo;
    public ObterClientePorIdHandler(IRepositorioLeituraCliente repo)
    {
        _repo = repo;
    }
    public Task<ClienteLeituraDto?> Handle(ObterClientePorIdQuery request, System.Threading.CancellationToken cancellationToken)
    {
        return _repo.ObterPorIdAsync(request.Id);
    }
}