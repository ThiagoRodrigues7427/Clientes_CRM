using System.Text.RegularExpressions;

namespace Clientes.Domain.Valores;

public sealed class Email
{
    public string Valor { get; }
    public Email(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor)) throw new ArgumentException("Email obrigatório");
        var padrao = new Regex("^[^@\n]+@[^@\n]+\\.[^@\n]+$");
        if (!padrao.IsMatch(valor)) throw new ArgumentException("Email inválido");
        Valor = valor.Trim();
    }
    public override string ToString() => Valor;
}