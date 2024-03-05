using System.Text.Json.Serialization;

namespace JustWatchSearch.Services.JustWatch.Responses;
public class TitleNodeWrapper
{
    [JsonPropertyName("node")]
    public TitleNode? Node { get; set; }
}
public class TitleNode
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("objectId")]
    public int ObjectId { get; set; }

    [JsonPropertyName("objectType")]
    public string ObjectType { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public TitleContent? Content { get; set; }


    public class TitleContent
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("fullPath")]
        public string FullPath { get; set; } = string.Empty;

        [JsonPropertyName("originalReleaseYear")]
        public int OriginalReleaseYear { get; set; }

        [JsonPropertyName("originalReleaseDate")]
        public string? OriginalReleaseDate { get; set; }

        [JsonPropertyName("runtime")]
        public int Runtime { get; set; }

        [JsonPropertyName("shortDescription")]
        public string? ShortDescription { get; set; }

        [JsonPropertyName("genres")]
        public List<Genre>? Genres { get; set; }

        [JsonPropertyName("externalIds")]
        public ExternalIds? ExternalIds { get; set; }

        [JsonPropertyName("posterUrl")]
        public string? PosterUrl { get; set; }

        [JsonPropertyName("backdrops")]
        public List<Backdrop>? Backdrops { get; set; }
    }

    public class Genre
    {
        [JsonPropertyName("shortName")]
        public string ShortName { get; set; } = string.Empty;
    }

    public class ExternalIds
    {
        [JsonPropertyName("imdbId")]
        public string? ImdbId { get; set; }
    }

    public class Backdrop
    {
        [JsonPropertyName("backdropUrl")]
        public string? BackdropUrl { get; set; }
    }
}