using System.Text.Json;
using System.Text.Json.Serialization;

namespace JustWatchSearch.Services;

public class CurrencyConverter : ICurrencyConverter
{
	private const string _apiUrl = "https://open.er-api.com/v6/latest/USD";
	private ExchangeRateResponse? _rates;
	private bool _initialized = false;

	public CurrencyConverter() { }

	public async Task InitializeAsync()
	{
		if (!_initialized)
		{
			await FetchExchangeRatesAsync();
			_initialized = true;
		}
	}

	private async Task FetchExchangeRatesAsync()
	{
		using (HttpClient client = new HttpClient())
		{
			HttpResponseMessage response = await client.GetAsync(_apiUrl);
			if (response.IsSuccessStatusCode)
			{
				string json = await response.Content.ReadAsStringAsync();
				_rates = JsonSerializer.Deserialize<ExchangeRateResponse>(json);
			}
			else
			{
				throw new Exception("Error fetching exchange rates.");
			}
		}
	}

	public decimal ConvertToUSD(string? currencyCode, decimal amount)
	{
		if (!_initialized || _rates == null)
		{
			throw new Exception("Currency converter not initialized.");
		}

		if (!string.IsNullOrEmpty(currencyCode) && _rates != null && _rates.Rates.ContainsKey(currencyCode))
		{
			decimal exchangeRate = _rates.Rates[currencyCode];
			return Math.Round(amount / exchangeRate, 2);
		}
		else
		{
			return amount == 0 ? 0 : 999;
		}
	}
}

public class ExchangeRateResponse
{
	[JsonPropertyName("base_code")]
	public string? BaseCode { get; set; }

	[JsonPropertyName("rates")]
	public Dictionary<string, decimal> Rates { get; set; } = new Dictionary<string, decimal>();
}
