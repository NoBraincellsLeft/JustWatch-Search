using GraphQL;

namespace JustWatchSearch.Services.JustWatch;

public static class JustWatchGraphQLQueries
{
  public static GraphQLRequest GetTitleOffersQuery(string nodeId, string country = "US")
  {
    return new GraphQLRequest
    {
      OperationName = "GetTitleOffers",
      Query = JustWatchGraphQLQueries._GetTitleOffersQuery,
      Variables = new
      {
        Country = country,
        filterBuy = new { },
        filterFlatrate = new { },
        filterFree = new { },
        filterRent = new { },
        language = "en",
        nodeId = nodeId,
        platform = "WEB"
      }
    };
  }

  public static GraphQLRequest GetSearchTitlesQuery(string input, string country = "US")
  {
    return new GraphQLRequest
    {
      OperationName = "GetSearchTitles",
      Query = JustWatchGraphQLQueries._getSearchTitlesQuery,
      Variables = new
      {
        searchTitlesFilter = new
        {
          searchQuery = input
        },
        country = country,
        language = "en",
        first = 10,
        formatPoster = "JPG",
        formatOfferIcon = "PNG",
        profile = "S718",
        backdropProfile = "S1920",
        filter = new
        {
        }
      }
    };
  }


  #region stringQueries
  public const string _getSearchTitlesQuery = @"
           query GetSearchTitles(
  $searchTitlesFilter: TitleFilter!,
  $country: Country!,
  $language: Language!,
  $first: Int!,
  $formatPoster: ImageFormat,
  $formatOfferIcon: ImageFormat,
  $profile: PosterProfile,
  $backdropProfile: BackdropProfile,
  $filter: OfferFilter!,
) {
  popularTitles(
    country: $country
    filter: $searchTitlesFilter
    first: $first
    sortBy: POPULAR
    sortRandomSeed: 0
  ) {
    edges {
      ...SearchTitleGraphql
      __typename
    }
    __typename
  }
}

fragment SearchTitleGraphql on PopularTitlesEdge {
  node {
    id
    objectId
    objectType
    content(country: $country, language: $language) {
      title
      fullPath
      originalReleaseYear
      originalReleaseDate
      runtime
      shortDescription
      genres {
        shortName
        __typename
      }
      externalIds {
        imdbId
        __typename
      }
      posterUrl(profile: $profile, format: $formatPoster)
      backdrops(profile: $backdropProfile, format: $formatPoster) {
        backdropUrl
        __typename
      }
      __typename
    }
    offers(country: $country, platform: WEB, filter: $filter) {
      monetizationType
      presentationType
      standardWebURL
      retailPrice(language: $language)
      retailPriceValue
      currency
      package {
        id
        packageId
        clearName
        technicalName
        icon(profile: S100, format: $formatOfferIcon)
        __typename
      }
      id
      __typename
    }
    __typename
  }
  __typename
}";

  public const string _GetTitleOffersQuery = @"query GetTitleOffers($nodeId: ID!, $country: Country!, $language: Language!, $filterFlatrate: OfferFilter!, $filterBuy: OfferFilter!, $filterRent: OfferFilter!, $filterFree: OfferFilter!, $platform: Platform! = WEB) {
  node(id: $nodeId) {
    id
    __typename
    ... on MovieOrShowOrSeasonOrEpisode {
      offerCount(country: $country, platform: $platform)
      maxOfferUpdatedAt(country: $country, platform: $platform)
      flatrate: offers(
        country: $country
        platform: $platform
        filter: $filterFlatrate
      ) {
        ...TitleOffer
        __typename
      }
      buy: offers(country: $country, platform: $platform, filter: $filterBuy) {
        ...TitleOffer
        __typename
      }
      rent: offers(country: $country, platform: $platform, filter: $filterRent) {
        ...TitleOffer
        __typename
      }
      free: offers(country: $country, platform: $platform, filter: $filterFree) {
        ...TitleOffer
        __typename
      }
      fast: offers(
        country: $country
        platform: $platform
        filter: {monetizationTypes: [FAST], bestOnly: true}
      ) {
        ...FastOffer
        __typename
      }
      __typename
    }
  }
}

fragment TitleOffer on Offer {
  id
  presentationType
  monetizationType
  retailPrice(language: $language)
  retailPriceValue
  currency
  lastChangeRetailPriceValue
  type
  package {
    id
    packageId
    clearName
    technicalName
    icon(profile: S100)
    __typename
  }
  standardWebURL
  elementCount
  availableTo
  deeplinkRoku: deeplinkURL(platform: ROKU_OS)
  subtitleLanguages
  videoTechnology
  audioTechnology
  audioLanguages
  __typename
}

fragment FastOffer on Offer {
  ...TitleOffer
  availableTo
  availableFromTime
  availableToTime
  __typename
}
";
}
#endregion
