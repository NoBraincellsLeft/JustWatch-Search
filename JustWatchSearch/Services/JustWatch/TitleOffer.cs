using static JustWatchSearch.Services.JustWatch.Responses.GetOffersResponse;
namespace JustWatchSearch.Services.JustWatch;
public class TitleOffer
{
    public string Country { get; set; }
    public FullOfferDetails OfferDetails { get; set; }
    public TitleOffer(FullOfferDetails offerDetails, string country)
    {
        Country = country;
        OfferDetails = offerDetails;
    }
}
