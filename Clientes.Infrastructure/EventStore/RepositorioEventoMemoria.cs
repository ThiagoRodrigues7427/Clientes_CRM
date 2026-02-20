using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clientes.Application.Abstracoes;

namespace Clientes.Infrastructure.EventStore;

public sealed class RepositorioEventoMemoria : IRepositorioEvento
{
    private static readonly ConcurrentDictionary<Guid, List<EventoDto>> _eventos = new();
    public Task AppendAsync(Guid aggregateId, string tipoEvento, string dadosJson, string usuario)
    {
        var lista = _eventos.GetOrAdd(aggregateId, _ => new List<EventoDto>());
        lista.Add(new EventoDto
        {
            Id = Guid.NewGuid(),
            AggregateId = aggregateId,
            Tipo = tipoEvento,
            Dados = dadosJson,
            Usuario = usuario,
            DataEvento = DateTime.UtcNow
        });
        return Task.CompletedTask;
    }
    public Task<IReadOnlyList<EventoDto>> ListarPorAggregateAsync(Guid aggregateId)
    {
        var lista = _eventos.TryGetValue(aggregateId, out var l) ? l : new List<EventoDto>();
        return Task.FromResult((IReadOnlyList<EventoDto>)lista.OrderBy(e => e.DataEvento).ToList());
    }
}