
namespace JustWatchSearch.Services;

public interface ICurrencyConverter
{
	decimal ConvertToUSD(string currencyCode, decimal amount);
	Task InitializeAsync();
}