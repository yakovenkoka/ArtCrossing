using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using ArtCrossing.Models;

namespace ArtCrossing.Services;

public class MetArtworkSource : IArtworkSource
{
    private const string SearchUrl = "https://collectionapi.metmuseum.org/public/collection/v1.1/search";
    private const string ObjectUrl = "https://collectionapi.metmuseum.org/public/collection/v1/objects/";
    private const int CandidateCount = 20;
    private const int MaxResults = 10;

    private readonly HttpClient _httpClient;

    public MetArtworkSource(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public MuseumEnum Museum => MuseumEnum.MetropolitanMuseumOfArt;

    public async Task<IReadOnlyList<Artwork>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        string url = $"{SearchUrl}?q={Uri.EscapeDataString(query)}&hasImages=true&limit={CandidateCount}";

        MetSearchResponse? response;
        try
        {
            response = await _httpClient.GetFromJsonAsync<MetSearchResponse>(url, cancellationToken);
        }
        catch (HttpRequestException ex)
        {
            throw new ArtworkSourceException(Museum, "The Met is not available right now.", ex);
        }
        catch (Exception ex) when (ex is JsonException or NotSupportedException)
        {
            throw new ArtworkSourceException(Museum, "The Met sent a response that could not be read.", ex);
        }

        if (response?.ObjectIds is null)
        {
            return [];
        }

        MetObject?[] objects = await Task.WhenAll(response.ObjectIds.Select(id => GetObjectAsync(id, cancellationToken)));

        return objects
            .OfType<MetObject>()
            .Where(item => !string.IsNullOrWhiteSpace(item.PrimaryImageSmall))
            .Take(MaxResults)
            .Select(ToArtwork)
            .ToList();
    }

    private async Task<MetObject?> GetObjectAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<MetObject>($"{ObjectUrl}{id}", cancellationToken);
        }
        catch (Exception ex) when (ex is HttpRequestException or JsonException or NotSupportedException)
        {
            return null;
        }
    }

    private Artwork ToArtwork(MetObject item)
    {
        string title = string.IsNullOrWhiteSpace(item.Title) ? "Untitled" : item.Title;

        return new Artwork(
            Museum,
            item.ObjectId.ToString(CultureInfo.InvariantCulture),
            title,
            string.IsNullOrWhiteSpace(item.ArtistDisplayName) ? "Unknown artist" : item.ArtistDisplayName,
            item.ObjectEndDate,
            item.Medium ?? string.Empty,
            item.PrimaryImageSmall ?? string.Empty,
            title);
    }

    private sealed record MetSearchResponse(List<int>? ObjectIds);

    private sealed record MetObject(
        int ObjectId,
        string? Title,
        string? ArtistDisplayName,
        int? ObjectEndDate,
        string? Medium,
        string? PrimaryImageSmall);
}
