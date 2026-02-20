using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Clientes.Application.Abstracoes;
using Polly;
using Polly.Retry;

namespace Clientes.Infrastructure.Servicos;

public sealed class ServicoViaCep : IServicoViaCep
{
    private readonly HttpClient _http;
    private readonly AsyncRetryPolicy _retry;
    public ServicoViaCep(HttpClient http)
    {
        _http = http;
        _retry = Policy.Handle<HttpRequestException>().WaitAndRetryAsync(3, i => System.TimeSpan.FromMilliseconds(200 * i));
    }
    public Task<ViaCepResposta?> ObterEnderecoAsync(string cep)
    {
        return _retry.ExecuteAsync(async () =>
        {
            var resp = await _http.GetFromJsonAsync<ViaCepResposta>($"https://viacep.com.br/ws/{cep}/json/");
            return resp;
        });
    }
}