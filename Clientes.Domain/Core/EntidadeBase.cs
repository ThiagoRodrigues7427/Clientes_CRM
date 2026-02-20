using System;
using System.Collections.Generic;

namespace Clientes.Domain.Core;

public abstract class EntidadeBase
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    private readonly List<object> _eventos = new();
    public IReadOnlyCollection<object> Eventos => _eventos.AsReadOnly();
    protected void AdicionarEvento(object evento) => _eventos.Add(evento);
    public void LimparEventos() => _eventos.Clear();
}