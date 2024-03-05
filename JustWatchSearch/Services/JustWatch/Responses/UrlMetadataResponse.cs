using System.Text.Json.Serialization;

namespace JustWatchSearch.Services.JustWatch.Responses;

public class UrlMetadataResponse
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("locale")]
	public string? Locale { get; set; }

	[JsonPropertyName("object_type")]
	public string? ObjectType { get; set; }

	[JsonPropertyName("object_id")]
	public int ObjectId { get; set; }

	[JsonPropertyName("full_path")]
	public string? FullPath { get; set; }

	[JsonPropertyName("heading_1")]
	public string? Heading1 { get; set; }

	[JsonPropertyName("meta_description")]
	public string? MetaDescription { get; set; }

	[JsonPropertyName("redirect_to_id")]
	public int RedirectToId { get; set; }

	[JsonPropertyName("temp_redirect_to_id")]
	public int TempRedirectToId { get; set; }

	[JsonPropertyName("meta_robots")]
	public string? MetaRobots { get; set; }

	[JsonPropertyName("meta_title")]
	public string? MetaTitle { get; set; }

	[JsonPropertyName("meta_keywords")]
	public string? MetaKeywords { get; set; }

	[JsonPropertyName("heading_2")]
	public string? Heading2 { get; set; }

	[JsonPropertyName("html_content")]
	public object? HtmlContent { get; set; }

	[JsonPropertyName("scripts")]
	public object? Scripts { get; set; }

	[JsonPropertyName("href_lang_tags")]
	public List<HrefLangTag>? HrefLangTags { get; set; }

	[JsonPropertyName("redirect_to_path")]
	public string? RedirectToPath { get; set; }

	[JsonPropertyName("i18n_state")]
	public string? I18nState { get; set; }

	[JsonPropertyName("parent_object_type")]
	public string? ParentObjectType { get; set; }

	[JsonPropertyName("parent_object_id")]
	public int ParentObjectId { get; set; }

	public class HrefLangTag
	{
		[JsonPropertyName("locale")]
		public string Locale { get; set; } = string.Empty;

		[JsonPropertyName("href_lang")]
		public string? HrefLang { get; set; }

		[JsonPropertyName("href")]
		public string? Href { get; set; }
	}
}
