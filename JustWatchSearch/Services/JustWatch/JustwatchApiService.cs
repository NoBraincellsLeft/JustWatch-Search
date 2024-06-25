using GraphQL.Client.Http;
using GraphQL.Client.Serializer.SystemTextJson;
using JustWatchSearch.Models;
using JustWatchSearch.Services.JustWatch.Responses;
using System.Text.Json;
using System.Web;
using static JustWatchSearch.Services.JustWatch.Responses.SearchTitlesResponse;
namespace JustWatchSearch.Services.JustWatch;

public partial class JustwatchApiService : IJustwatchApiService
{
	private readonly GraphQLHttpClient _graphQLClient;
	private readonly ILogger<JustwatchApiService> _logger;
	private readonly ICurrencyConverter _currencyConverter;
	private readonly string _baseAddress = "https://cors-proxy.cooks.fyi/https://apis.justwatch.com";

	public JustwatchApiService(ILogger<JustwatchApiService> logger, ICurrencyConverter currencyConverter)
	{
		_logger = logger;
		_currencyConverter = currencyConverter;
		_graphQLClient = new GraphQLHttpClient($"{_baseAddress}/graphql", new SystemTextJsonSerializer());
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

	public async Task<GetOffersResponse?> GetTitleOffers(string id, IEnumerable<string> countries, CancellationToken? token)
	{
		_logger.LogInformation("Got Title Offer request {id}", id);
		try
		{
			var searchResult = await _graphQLClient.SendQueryAsync<GetOffersResponse>(JustWatchGraphQLQueries.GetTitleOffersQuery(id, countries), token ?? default);
			return searchResult.Data;
		}
		catch (TaskCanceledException) { throw; }
		catch (Exception ex)
		{
			_logger.LogError(" Get title Offers request {id} failed with {ex}", id, ex);
			return null;
		}
	}



	public async Task<UrlMetadataResponse?> GetUrlMetadataResponseAsync(string path)
	{
		using (var httpClient = new HttpClient())
		{
			string url = $"{_baseAddress}/content/urls?path={HttpUtility.UrlEncode(path)}";
			var response = await httpClient.GetAsync(url);
			var urlMetadataResponse = JsonSerializer.Deserialize<UrlMetadataResponse>(await response.Content.ReadAsStringAsync());
			return urlMetadataResponse;
		}
	}

	public async Task<string[]> GetAvaibleLocales(string path)
	{
		var urlMetadataResponse = await GetUrlMetadataResponseAsync(path);
		return urlMetadataResponse?.HrefLangTags?.Select(tag => tag.Locale)?.ToArray() ?? new string[0];
	}

	public async Task<IEnumerable<TitleOfferViewModel>?> GetAllOffers(string nodeId, string path, CancellationToken? token)
	{
		await _currencyConverter.InitializeAsync();
		var locales = await GetAvaibleLocales(path);
		_logger.LogInformation("Got locale {locales}", locales);

		var countries = locales.Select(o => o.Split("_").Last());
		var titleOffer = await GetTitleOffers(nodeId, countries, token);
		if (titleOffer == null)
			return null;

		return titleOffer.Offers.SelectMany(item => item.Value.Select(o => new TitleOfferViewModel(o, item.Key, _currencyConverter.ConvertToUSD(o.Currency, o.RetailPriceValue ?? 0))));

	}

	public async Task<TitleNode?> GetTitle(string nodeId, CancellationToken? token = null)
	{
		try
		{
			var searchResult = await _graphQLClient.SendQueryAsync<TitleNodeWrapper>(JustWatchGraphQLQueries.GetTitleNode(nodeId), token ?? default);
			return searchResult.Data?.Node;
		}
		catch (TaskCanceledException) { throw; }
		catch (Exception ex)
		{
			_logger.LogError(" Get title Offers request {id} failed with {ex}", nodeId, ex);
			return null;
		}
	}
}
