using ArtCrossing.Models;

namespace ArtCrossing.Tests.Models;

public class ArtworkTests
{
    [Fact]
    public void CompareTo_EarlierYear_SortsFirst()
    {
        Artwork older = CreateArtwork("Water Lilies", 1906);
        Artwork newer = CreateArtwork("Nighthawks", 1942);

        Assert.True(older.CompareTo(newer) < 0);
        Assert.True(newer.CompareTo(older) > 0);
    }

    [Fact]
    public void CompareTo_UnknownYear_SortsLast()
    {
        Artwork unknownYear = CreateArtwork("Untitled", null);
        Artwork knownYear = CreateArtwork("Nighthawks", 1942);

        Assert.True(unknownYear.CompareTo(knownYear) > 0);
        Assert.True(knownYear.CompareTo(unknownYear) < 0);
    }

    [Fact]
    public void CompareTo_SameYear_SortsByTitleIgnoringCase()
    {
        Artwork first = CreateArtwork("a Sunday Afternoon", 1884);
        Artwork second = CreateArtwork("Bathers at Asnieres", 1884);

        Assert.True(first.CompareTo(second) < 0);
    }

    [Fact]
    public void Sort_MixedArtworks_OrdersByYearThenTitleWithUnknownYearsLast()
    {
        List<Artwork> artworks =
        [
            CreateArtwork("Untitled", null),
            CreateArtwork("Nighthawks", 1942),
            CreateArtwork("The Bedroom", 1889),
            CreateArtwork("Starry Night", 1889)
        ];

        artworks.Sort();

        Assert.Equal(["Starry Night", "The Bedroom", "Nighthawks", "Untitled"], artworks.Select(artwork => artwork.Title));
    }

    [Fact]
    public void Key_CombinesMuseumAndSourceId()
    {
        Artwork artwork = CreateArtwork("Nighthawks", 1942, MuseumEnum.ArtInstituteOfChicago, "111628");

        Assert.Equal("ArtInstituteOfChicago-111628", artwork.Key);
    }

    [Theory]
    [InlineData(MuseumEnum.ArtInstituteOfChicago, "https://www.artic.edu/artworks/42")]
    [InlineData(MuseumEnum.MetropolitanMuseumOfArt, "https://www.metmuseum.org/art/collection/search/42")]
    [InlineData(MuseumEnum.Undefined, "")]
    public void WebUrl_ReturnsArtworkPageOnMuseumWebsite(MuseumEnum museum, string expectedUrl)
    {
        Artwork artwork = CreateArtwork("Nighthawks", 1942, museum, "42");

        Assert.Equal(expectedUrl, artwork.WebUrl);
    }

    private static Artwork CreateArtwork(string title, int? year,
        MuseumEnum museum = MuseumEnum.ArtInstituteOfChicago, string sourceId = "1")
    {
        return new Artwork(museum, sourceId, title, "Artist", year, "Oil on canvas", string.Empty, title);
    }
}
