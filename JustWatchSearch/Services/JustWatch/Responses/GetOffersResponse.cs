using System.Text.Json.Serialization;

namespace JustWatchSearch.Services.JustWatch.Responses;


public class GetOffersResponse
{

	[JsonPropertyName("node")]
	public MovieNode TitleNode { get; set; }

	public class MovieNode
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("__typename")]
		public string TypeName { get; set; }

		[JsonPropertyName("offerCount")]
		public int OfferCount { get; set; }

		[JsonPropertyName("maxOfferUpdatedAt")]
		public string MaxOfferUpdatedAt { get; set; }

		[JsonPropertyName("flatrate")]
		public List<object> FlatrateOffers { get; set; }

		[JsonPropertyName("buy")]
		public List<FullOfferDetails> BuyOffers { get; set; }

		[JsonPropertyName("rent")]
		public List<object> RentOffers { get; set; }

		[JsonPropertyName("free")]
		public List<object> FreeOffers { get; set; }

		[JsonPropertyName("fast")]
		public List<object> FastOffers { get; set; }
	}

	public class FullOfferDetails
	{
		[JsonPropertyName("id")]
		public string Id { get; set; }

		[JsonPropertyName("presentationType")]
		public string PresentationType { get; set; }

		[JsonPropertyName("monetizationType")]
		public string MonetizationType { get; set; }

		[JsonPropertyName("retailPrice")]
		public string RetailPrice { get; set; }

		[JsonPropertyName("retailPriceValue")]
		public double? RetailPriceValue { get; set; }

		[JsonPropertyName("currency")]
		public string Currency { get; set; }

		[JsonPropertyName("lastChangeRetailPriceValue")]
		public double? LastChangeRetailPriceValue { get; set; }

		[JsonPropertyName("type")]
		public string Type { get; set; }

		[JsonPropertyName("package")]
		public OfferPackage Package { get; set; }

		[JsonPropertyName("standardWebURL")]
		public string StandardWebUrl { get; set; }

		[JsonPropertyName("elementCount")]
		public int ElementCount { get; set; }

		[JsonPropertyName("availableTo")]
		public object AvailableTo { get; set; }

		[JsonPropertyName("deeplinkRoku")]
		public string DeeplinkRoku { get; set; }

		[JsonPropertyName("subtitleLanguages")]
		public List<string> SubtitleLanguages { get; set; }

		[JsonPropertyName("videoTechnology")]
		public List<string> VideoTechnology { get; set; }

		[JsonPropertyName("audioTechnology")]
		public List<object> AudioTechnology { get; set; }

		[JsonPropertyName("audioLanguages")]
		public List<string> AudioLanguages { get; set; }
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

		[JsonPropertyName("__typename")]
		public string TypeName { get; set; }
	}
}
