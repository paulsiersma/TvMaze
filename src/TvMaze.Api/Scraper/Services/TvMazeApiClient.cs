using System.Net;
using System.Text.Json;
using TvMaze.Scraper.Models;

namespace TvMaze.Scraper.Services;

public class TvMazeApiClient: ITvMazeApiClient
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<TvMazeApiClient> _logger;
    private const int RateLimitPeriod = 10000;
    private const int RateLimitMaxCallsPerPeriod = 20;

    public TvMazeApiClient(IHttpClientFactory httpClientFactory, ILogger<TvMazeApiClient> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<IEnumerable<TvMazeApiShowModel>> ScrapeShows(int higherThanId)
    {
        var id = higherThanId + 1;
        var shows = new List<TvMazeApiShowModel>();

        for (int i = 0; i <= RateLimitMaxCallsPerPeriod; i++)
        {
            var show = await GetShow(id);

            if (show != null)
            {
                shows.Add(show);
            }

            id++;

            await Task.Delay(RateLimitPeriod / RateLimitMaxCallsPerPeriod);
        }

        return shows;
    }

    private async Task<TvMazeApiShowModel?> GetShow(int id)
    {
        var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            $"https://api.tvmaze.com/shows/{id}?embed=cast");

        var httpClient = _httpClientFactory.CreateClient();
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        if (!httpResponseMessage.IsSuccessStatusCode)
        {
            _logger.LogWarning(httpResponseMessage.StatusCode == HttpStatusCode.TooManyRequests
                ? "Rate limit exceeded!"
                : $"Request for id {id} resulted in non-success status code: {httpResponseMessage.StatusCode}");
            return null;
        }
            
        var stringResult = await httpResponseMessage.Content.ReadAsStringAsync();

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var result = JsonSerializer.Deserialize<TvMazeApiShowModel>(stringResult, options);
        return result;
    }
}

public interface ITvMazeApiClient
{
    Task<IEnumerable<TvMazeApiShowModel>> ScrapeShows(int higherThanId);
}
