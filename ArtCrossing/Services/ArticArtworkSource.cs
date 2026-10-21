using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using ArtCrossing.Models;

namespace ArtCrossing.Services;

public class ArticArtworkSource : IArtworkSource
{
    private const string SearchUrl = "https://api.artic.edu/api/v1/artworks/search";
    private const string Fields = "id,title,artist_title,date_end,medium_display,image_id,thumbnail";
    private const int MaxResults = 10;
    private const string ImageSize = "full/843,/0/default.jpg";
    private const string UserAgent = "ArtCrossing/1.0";

    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    private readonly HttpClient _httpClient;

    public ArticArtworkSource(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
    }

    public MuseumEnum Museum => MuseumEnum.ArtInstituteOfChicago;

    public async Task<IReadOnlyList<Artwork>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        string url = $"{SearchUrl}?q={Uri.EscapeDataString(query)}&limit={MaxResults}&fields={Fields}";

        ArticSearchResponse? response;
        try
        {
            response = await _httpClient.GetFromJsonAsync<ArticSearchResponse>(url, JsonOptions, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new ArtworkSourceException(Museum, "The Art Institute of Chicago is not available right now.", ex);
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
            throw new ArtworkSourceException(Museum, "The Art Institute of Chicago sent a response that could not be read.", ex);
        }

        if (response?.Data is null)
        {
            return [];
        }

        string? imageServer = response.Config?.IiifUrl;
        return response.Data.Select(item => ToArtwork(item, imageServer)).ToList();
    }

    private Artwork ToArtwork(ArticArtwork item, string? imageServer)
    {
        string imageUrl = imageServer is not null && item.ImageId is not null
            ? $"{imageServer}/{item.ImageId}/{ImageSize}"
            : string.Empty;

        string title = string.IsNullOrWhiteSpace(item.Title) ? "Untitled" : item.Title;

        return new Artwork(
            Museum,
            item.Id.ToString(CultureInfo.InvariantCulture),
            title,
            string.IsNullOrWhiteSpace(item.ArtistTitle) ? "Unknown artist" : item.ArtistTitle,
            item.DateEnd,
            item.MediumDisplay ?? string.Empty,
            imageUrl,
            item.Thumbnail?.AltText ?? title);
    }

    private sealed record ArticSearchResponse(List<ArticArtwork>? Data, ArticConfig? Config);

    private sealed record ArticConfig(string? IiifUrl);

    private sealed record ArticArtwork(
        int Id,
        string? Title,
        string? ArtistTitle,
        int? DateEnd,
        string? MediumDisplay,
        string? ImageId,
        ArticThumbnail? Thumbnail);

    private sealed record ArticThumbnail(string? AltText);
}
