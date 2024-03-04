﻿using JustWatchSearch.Services.JustWatch.Responses;
using static JustWatchSearch.Services.JustWatch.Responses.SearchTitlesResponse;

namespace JustWatchSearch.Services.JustWatch;
public interface IJustwatchApiService
{
    Task<IEnumerable<TitleOffer>> GetAllOffers(string nodeId, string path, CancellationToken? token = null);
    Task<SearchTitlesResponse> SearchTitlesAsync(string input, CancellationToken? token = null);
    Task<TitleNode> GetTitle(string nodeId, string country = "us", CancellationToken? token = null);
    Task<UrlMetadataResponse?> GetUrlMetadataResponseAsync(string path);
}