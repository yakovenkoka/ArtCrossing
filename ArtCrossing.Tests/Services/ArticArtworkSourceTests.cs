using System.Net;
using ArtCrossing.Models;
using ArtCrossing.Services;
using ArtCrossing.Tests.Fakes;

namespace ArtCrossing.Tests.Services;

public class ArticArtworkSourceTests
{
    private const string SearchResponse = """
        {
            "data": [
                {
                    "id": 111628,
                    "title": "Nighthawks",
                    "artist_title": "Edward Hopper",
                    "date_end": 1942,
                    "medium_display": "Oil on canvas",
                    "image_id": "831a05de",
                    "thumbnail": { "alt_text": "A diner at night" }
                },
                {
                    "id": 2,
                    "title": null,
                    "artist_title": null,
                    "date_end": null,
                    "medium_display": null,
                    "image_id": null,
                    "thumbnail": null
                }
            ],
            "config": { "iiif_url": "https://www.artic.edu/iiif/2" }
        }
        """;

    [Fact]
    public async Task SearchAsync_ValidResponse_MapsArtworks()
    {
        ArticArtworkSource source = CreateSource(_ => FakeHttpMessageHandler.Json(SearchResponse));

        IReadOnlyList<Artwork> artworks = await source.SearchAsync("nighthawks");

        Assert.Collection(artworks,
            nighthawks =>
            {
                Assert.Equal(MuseumEnum.ArtInstituteOfChicago, nighthawks.Museum);
                Assert.Equal("111628", nighthawks.SourceId);
                Assert.Equal("Nighthawks", nighthawks.Title);
                Assert.Equal("Edward Hopper", nighthawks.Artist);
                Assert.Equal(1942, nighthawks.CompletionYear);
                Assert.Equal("Oil on canvas", nighthawks.Medium);
                Assert.Equal("https://www.artic.edu/iiif/2/831a05de/full/843,/0/default.jpg", nighthawks.ImageUrl);
                Assert.Equal("A diner at night", nighthawks.AltText);
            },
            missingData =>
            {
                Assert.Equal("Untitled", missingData.Title);
                Assert.Equal("Unknown artist", missingData.Artist);
                Assert.Null(missingData.CompletionYear);
                Assert.Equal(string.Empty, missingData.ImageUrl);
            });
    }

    [Fact]
    public async Task SearchAsync_ServerError_ThrowsArtworkSourceException()
    {
        ArticArtworkSource source = CreateSource(_ => FakeHttpMessageHandler.Status(HttpStatusCode.InternalServerError));

        ArtworkSourceException exception =
            await Assert.ThrowsAsync<ArtworkSourceException>(() => source.SearchAsync("nighthawks"));

        Assert.Equal(MuseumEnum.ArtInstituteOfChicago, exception.Museum);
    }

    [Fact]
    public async Task SearchAsync_InvalidJson_ThrowsArtworkSourceException()
    {
        ArticArtworkSource source = CreateSource(_ => FakeHttpMessageHandler.Json("not json"));

        await Assert.ThrowsAsync<ArtworkSourceException>(() => source.SearchAsync("nighthawks"));
    }

    private static ArticArtworkSource CreateSource(Func<HttpRequestMessage, HttpResponseMessage> respond)
    {
        return new ArticArtworkSource(new HttpClient(new FakeHttpMessageHandler(respond)));
    }
}
