namespace Clientes.Domain.Valores;

public sealed class Endereco
{
    public string Cep { get; }
    public string Logradouro { get; }
    public string Numero { get; }
    public string Bairro { get; }
    public string Cidade { get; }
    public string Estado { get; }

    public Endereco(string cep, string logradouro, string numero, string bairro, string cidade, string estado)
    {
        Cep = cep;
        Logradouro = logradouro;
        Numero = numero;
        Bairro = bairro;
        Cidade = cidade;
        Estado = estado;
        if (string.IsNullOrWhiteSpace(Cep)) throw new ArgumentException("CEP obrigatório");
        if (string.IsNullOrWhiteSpace(Logradouro)) throw new ArgumentException("Logradouro obrigatório");
        if (string.IsNullOrWhiteSpace(Numero)) throw new ArgumentException("Número obrigatório");
        if (string.IsNullOrWhiteSpace(Bairro)) throw new ArgumentException("Bairro obrigatório");
        if (string.IsNullOrWhiteSpace(Cidade)) throw new ArgumentException("Cidade obrigatória");
        if (string.IsNullOrWhiteSpace(Estado)) throw new ArgumentException("Estado obrigatório");
    }
}