using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Clientes.Application.Abstracoes;
using MediatR;

namespace Clientes.Application.Clientes;

public sealed class ListarClientesQuery : IRequest<IReadOnlyList<ClienteLeituraDto>> {}

public sealed class ListarClientesHandler : IRequestHandler<ListarClientesQuery, IReadOnlyList<ClienteLeituraDto>>
{
    private readonly IRepositorioLeituraCliente _repo;
    public ListarClientesHandler(IRepositorioLeituraCliente repo)
    {
        _repo = repo;
    }
    public Task<IReadOnlyList<ClienteLeituraDto>> Handle(ListarClientesQuery request, CancellationToken cancellationToken)
    {
        return _repo.ListarAsync();
    }
}