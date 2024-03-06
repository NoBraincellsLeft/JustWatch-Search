using System.Text.Json.Serialization;

namespace JustWatchSearch.Services.JustWatch.Responses;


public class GetOffersResponse
{
    [JsonPropertyName("node")]
    public IDictionary<string, List<OfferDetails>> Offers { get; set; } = new Dictionary<string, List<OfferDetails>>();
}
