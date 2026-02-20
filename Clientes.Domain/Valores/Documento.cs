using System.Text.RegularExpressions;

namespace Clientes.Domain.Valores;

public sealed class Documento
{
    public string Valor { get; }
    public bool EhCpf { get; }

    public Documento(string valor)
    {
        var apenasNumeros = Regex.Replace(valor ?? string.Empty, "[^0-9]", "");
        Valor = apenasNumeros;
        EhCpf = Valor.Length <= 11;
        if (EhCpf)
        {
            if (Valor.Length != 11) throw new ArgumentException("CPF inválido");
        }
        else
        {
            if (Valor.Length != 14) throw new ArgumentException("CNPJ inválido");
        }
    }
    public override string ToString() => Valor;
}