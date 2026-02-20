using System;
using System.Threading.Tasks;
using Clientes.Application.Abstracoes;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace Clientes.Infrastructure.EventStore;

public sealed class RepositorioEvento : IRepositorioEvento
{
    private readonly string _connString;
    private readonly DbContext _ctx;
    public RepositorioEvento(DbContext ctx)
    {
        _ctx = ctx;
        _connString = ctx.Database.GetConnectionString() ?? string.Empty;
    }
    public async Task AppendAsync(Guid aggregateId, string tipoEvento, string dadosJson, string usuario)
    {
        await GarantirTabelaAsync();
        await using var con = new NpgsqlConnection(_connString);
        var sql = "insert into eventos (id, aggregate_id, tipo, dados, usuario, data_evento) values (@id, @aggregate_id, @tipo, @dados::jsonb, @usuario, @data_evento)";
        await con.ExecuteAsync(sql, new
        {
            id = Guid.NewGuid(),
            aggregate_id = aggregateId,
            tipo = tipoEvento,
            dados = dadosJson,
            usuario,
            data_evento = DateTime.UtcNow
        });
    }
    async Task GarantirTabelaAsync()
    {
        await using var con = new NpgsqlConnection(_connString);
        var sql = "create table if not exists eventos (id uuid primary key, aggregate_id uuid not null, tipo text not null, dados jsonb not null, usuario text not null, data_evento timestamptz not null); create index if not exists idx_eventos_aggregate on eventos(aggregate_id);";
        await con.ExecuteAsync(sql);
    }
    public async Task<IReadOnlyList<EventoDto>> ListarPorAggregateAsync(Guid aggregateId)
    {
        await using var con = new NpgsqlConnection(_connString);
        var sql = "select id as Id, aggregate_id as AggregateId, tipo as Tipo, dados::text as Dados, usuario as Usuario, data_evento as DataEvento from eventos where aggregate_id = @aggregate order by data_evento asc";
        var lista = await con.QueryAsync<EventoDto>(sql, new { aggregate = aggregateId });
        return lista.AsList();
    }
}