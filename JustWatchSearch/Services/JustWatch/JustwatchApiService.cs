using JustWatchSearch.Services.JustWatch.Responses;
using GraphQL;
using GraphQL.Client.Abstractions;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using System.Text.Json;
using System.Web;
using static JustWatchSearch.Services.JustWatch.Responses.SearchTitlesResponse;
namespace JustWatchSearch.Services.JustWatch;

public partial class JustwatchApiService : IJustwatchApiService
{
    private readonly GraphQLHttpClient _graphQLClient;
    private readonly ILogger<JustwatchApiService> _logger;

    public JustwatchApiService(ILogger<JustwatchApiService> logger)
    {
        _logger = logger;
        _graphQLClient = new GraphQLHttpClient("https://apis.justwatch.com/graphql", new SystemTextJsonSerializer());
    }

    public async Task<SearchTitlesResponse> SearchTitlesAsync(string input, CancellationToken? token)
    {
        try
        {
            var searchResult = await _graphQLClient.SendQueryAsync<SearchTitlesResponse>(JustWatchGraphQLQueries.GetSearchTitlesQuery(input), token ?? default);
            return searchResult.Data;
        }
        catch (TaskCanceledException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError("Searching title {input} failed with {ex}", input, ex);
            throw;
        }
    }

    public async Task<GetOffersResponse> GetTitleOffers(string id, string country, CancellationToken? token)
    {
        _logger.LogInformation("Got Title Offer request {id}", id);
        try
        {
            var searchResult = await _graphQLClient.SendQueryAsync<GetOffersResponse>(JustWatchGraphQLQueries.GetTitleOffersQuery(id, country), token ?? default);
            _logger.LogInformation("Got Offer Result {id} Errors: {Length} Offers: {Offers}", id, searchResult.Errors?.Length ?? 0, searchResult.Data.TitleNode?.OfferCount);
            return searchResult.Data;
        }
        catch (TaskCanceledException) { throw; }
        catch (Exception ex)
        {
            _logger.LogError("Searching title {input} failed with {ex}", id, ex);
            throw;
        }
    }

    public async Task<UrlMetadataResponse?> GetUrlMetadataResponseAsync(string path)
    {
        using (var httpClient = new HttpClient())
        {
            string url = $"https://apis.justwatch.com/content/urls?path={HttpUtility.UrlEncode(path)}";
            var response = await httpClient.GetAsync(url);
            var urlMetadataResponse = JsonSerializer.Deserialize<UrlMetadataResponse>(await response.Content.ReadAsStringAsync());
            return urlMetadataResponse;
        }
    }

    public async Task<string[]> GetAvaibleLocales(string path)
    {
        var urlMetadataResponse = await GetUrlMetadataResponseAsync(path);
        return urlMetadataResponse?.HrefLangTags?.Select(tag => tag.Locale).ToArray() ?? new string[0];
    }

    public async Task<IEnumerable<TitleOffer>> GetAllOffers(string nodeId, string path, CancellationToken? token)
    {
        var locales = await GetAvaibleLocales(path);
        _logger.LogInformation("Got locale {locales}", locales);
        var result = new List<TitleOffer>();
        foreach (var locale in locales)
        {
            var country = locale.Split("_").Last();
            var titleOffer = await GetTitleOffers(nodeId, country, token);
            var temp = titleOffer.TitleNode.BuyOffers.Select(o => new TitleOffer(o, country));
            result.AddRange(temp);
        }
        return result;
    }

    public async Task<TitleNode> GetTitle(string nodeId, string country = "us", CancellationToken? token = null)
    {
        GetOffersResponse titleOffer = await GetTitleOffers(nodeId, country, token);
       throw new NotImplementedException();



    }
}
