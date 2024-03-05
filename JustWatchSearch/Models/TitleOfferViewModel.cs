using static JustWatchSearch.Services.JustWatch.Responses.GetOffersResponse;
namespace JustWatchSearch.Models;
public class TitleOfferViewModel
{
	public string Country { get; set; }

	public string? PackageURL => OfferDetails?.StandardWebUrl;

	public string? PackageClearName => OfferDetails?.Package?.ClearName;
	public string? RetailPrice => OfferDetails?.RetailPrice;
	public decimal? RetailPriceValue => OfferDetails?.RetailPriceValue;
	public decimal NormalizedPrice { get; init; }
	public string? PresentationType => OfferDetails?.PresentationType;
	public string? MonetizationType => OfferDetails?.MonetizationType;

	public OfferDetails OfferDetails { get; set; }


	public TitleOfferViewModel(OfferDetails offerDetails, string country, decimal usdPrice)
	{
		Country = country;
		OfferDetails = offerDetails;
		NormalizedPrice = usdPrice;
	}
}
