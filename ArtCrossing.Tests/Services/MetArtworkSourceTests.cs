using System.Net;
using ArtCrossing.Models;
using ArtCrossing.Services;
using ArtCrossing.Tests.Fakes;

namespace ArtCrossing.Tests.Services;

public class MetArtworkSourceTests
{
    private const string SearchResponse = """{ "total": 3, "objectIDs": [436535, 2, 3] }""";

    private const string ObjectWithImage = """
        {
            "objectID": 436535,
            "title": "Wheat Field with Cypresses",
            "artistDisplayName": "Vincent van Gogh",
            "objectEndDate": 1889,
            "medium": "Oil on canvas",
            "primaryImageSmall": "https://images.metmuseum.org/DT1567.jpg"
        }
        """;

    private const string ObjectWithoutImage = """
        {
            "objectID": 2,
            "title": "Fragment",
            "artistDisplayName": "",
            "objectEndDate": 1500,
            "medium": "Stone",
            "primaryImageSmall": ""
        }
        """;

    [Fact]
    public async Task SearchAsync_ValidResponses_FetchesObjectsAndMapsArtworksWithImages()
    {
        MetArtworkSource source = CreateSource(request => request.RequestUri!.AbsolutePath switch
        {
            var path when path.EndsWith("/search") => FakeHttpMessageHandler.Json(SearchResponse),
            var path when path.EndsWith("/objects/436535") => FakeHttpMessageHandler.Json(ObjectWithImage),
            var path when path.EndsWith("/objects/2") => FakeHttpMessageHandler.Json(ObjectWithoutImage),
            _ => FakeHttpMessageHandler.Status(HttpStatusCode.NotFound)
        });

        IReadOnlyList<Artwork> artworks = await source.SearchAsync("cypresses");

        Artwork artwork = Assert.Single(artworks);
        Assert.Equal(MuseumEnum.MetropolitanMuseumOfArt, artwork.Museum);
        Assert.Equal("436535", artwork.SourceId);
        Assert.Equal("Wheat Field with Cypresses", artwork.Title);
        Assert.Equal("Vincent van Gogh", artwork.Artist);
        Assert.Equal(1889, artwork.CompletionYear);
        Assert.Equal("Oil on canvas", artwork.Medium);
        Assert.Equal("https://images.metmuseum.org/DT1567.jpg", artwork.ImageUrl);
    }

    [Fact]
    public async Task SearchAsync_NoMatches_ReturnsEmptyList()
    {
        MetArtworkSource source = CreateSource(_ => FakeHttpMessageHandler.Json("""{ "total": 0, "objectIDs": null }"""));

        IReadOnlyList<Artwork> artworks = await source.SearchAsync("zzzz");

        Assert.Empty(artworks);
    }

    [Fact]
    public async Task SearchAsync_ServerError_ThrowsArtworkSourceException()
    {
        MetArtworkSource source = CreateSource(_ => FakeHttpMessageHandler.Status(HttpStatusCode.InternalServerError));

        ArtworkSourceException exception =
            await Assert.ThrowsAsync<ArtworkSourceException>(() => source.SearchAsync("cypresses"));

        Assert.Equal(MuseumEnum.MetropolitanMuseumOfArt, exception.Museum);
    }

    private static MetArtworkSource CreateSource(Func<HttpRequestMessage, HttpResponseMessage> respond)
    {
        return new MetArtworkSource(new HttpClient(new FakeHttpMessageHandler(respond)));
    }
}
