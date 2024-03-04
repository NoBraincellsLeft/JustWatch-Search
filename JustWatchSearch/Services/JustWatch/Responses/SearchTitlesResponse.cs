using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace JustWatchSearch.Services.JustWatch.Responses
{
	public class SearchTitlesResponse
	{
		[JsonPropertyName("popularTitles")]
		public SearchResult Result { get; set; }

		public class SearchResult
		{
			[JsonPropertyName("edges")]
			public List<PopularTitlesEdge> TitleResults { get; set; }
		}

		public class PopularTitlesEdge
		{
			[JsonPropertyName("node")]
			public TitleNode Node { get; set; }
		}

		public class TitleNode
		{
			[JsonPropertyName("id")]
			public string Id { get; set; }

			[JsonPropertyName("objectId")]
			public int ObjectId { get; set; }

			[JsonPropertyName("objectType")]
			public string ObjectType { get; set; }

			[JsonPropertyName("content")]
			public TitleContent Content { get; set; }

			[JsonPropertyName("offers")]
			public List<OfferDetails> Offers { get; set; }
		}

		public class TitleContent
		{
			[JsonPropertyName("title")]
			public string Title { get; set; }

			[JsonPropertyName("fullPath")]
			public string FullPath { get; set; }

			[JsonPropertyName("originalReleaseYear")]
			public int OriginalReleaseYear { get; set; }

			[JsonPropertyName("originalReleaseDate")]
			public string OriginalReleaseDate { get; set; }

			[JsonPropertyName("runtime")]
			public int Runtime { get; set; }

			[JsonPropertyName("shortDescription")]
			public string ShortDescription { get; set; }

			[JsonPropertyName("genres")]
			public List<Genre> Genres { get; set; }

			[JsonPropertyName("externalIds")]
			public ExternalIds ExternalIds { get; set; }

			[JsonPropertyName("posterUrl")]
			public string PosterUrl { get; set; }

			[JsonPropertyName("backdrops")]
			public List<Backdrop> Backdrops { get; set; }
		}

		public class Genre
		{
			[JsonPropertyName("shortName")]
			public string ShortName { get; set; }
		}

		public class ExternalIds
		{
			[JsonPropertyName("imdbId")]
			public string ImdbId { get; set; }
		}

		public class Backdrop
		{
			[JsonPropertyName("backdropUrl")]
			public string BackdropUrl { get; set; }
		}

		public class OfferDetails
		{
			[JsonPropertyName("monetizationType")]
			public string MonetizationType { get; set; }

			[JsonPropertyName("presentationType")]
			public string ResolutionType { get; set; }

			[JsonPropertyName("standardWebURL")]
			public string StandardWebURL { get; set; }

			[JsonPropertyName("retailPrice")]
			public string RetailPrice { get; set; }

			[JsonPropertyName("retailPriceValue")]
			public double? RetailPriceValue { get; set; }

			[JsonPropertyName("currency")]
			public string Currency { get; set; }

			[JsonPropertyName("package")]
			public OfferPackage Package { get; set; }
		}

		public class OfferPackage
		{
			[JsonPropertyName("id")]
			public string Id { get; set; }

			[JsonPropertyName("packageId")]
			public int PackageId { get; set; }

			[JsonPropertyName("clearName")]
			public string ClearName { get; set; }

			[JsonPropertyName("technicalName")]
			public string TechnicalName { get; set; }

			[JsonPropertyName("icon")]
			public string Icon { get; set; }
		}
	}

}
