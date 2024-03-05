using System.Text.Json.Serialization;

namespace JustWatchSearch.Services.JustWatch.Responses;

public class SearchTitlesResponse
{
    [JsonPropertyName("popularTitles")]
    public SearchResult Result { get; set; } = new SearchResult();

    public class SearchResult
    {
        [JsonPropertyName("edges")]
        public List<TitleNodeWrapper> TitleResults { get; set; } = new List<TitleNodeWrapper>();
    }
}