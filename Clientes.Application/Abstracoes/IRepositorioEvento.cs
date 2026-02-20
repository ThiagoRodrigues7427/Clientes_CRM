using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Clientes.Application.Abstracoes;

public interface IRepositorioEvento
{
    Task AppendAsync(Guid aggregateId, string tipoEvento, string dadosJson, string usuario);
    Task<IReadOnlyList<EventoDto>> ListarPorAggregateAsync(Guid aggregateId);
}

public sealed class EventoDto
{
    public Guid Id { get; set; }
    public Guid AggregateId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Dados { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public DateTime DataEvento { get; set; }
}