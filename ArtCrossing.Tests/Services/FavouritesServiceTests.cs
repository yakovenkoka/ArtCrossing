using ArtCrossing.Models;
using ArtCrossing.Services;

namespace ArtCrossing.Tests.Services;

public class FavouritesServiceTests
{
    private readonly FavouritesService _favourites = new();

    [Fact]
    public void GetAll_NewService_ReturnsNoFavourites()
    {
        Assert.Empty(_favourites.GetAll());
    }

    [Fact]
    public void Toggle_ArtworkNotInFavourites_AddsAndStarsIt()
    {
        Artwork artwork = CreateArtwork("111628");

        _favourites.Toggle(artwork);

        Assert.Single(_favourites.GetAll(), artwork);
        Assert.True(_favourites.IsFavourite(artwork));
        Assert.True(artwork.Starred);
    }

    [Fact]
    public void Toggle_ArtworkAlreadyInFavourites_RemovesAndUnstarsIt()
    {
        Artwork artwork = CreateArtwork("111628");
        _favourites.Toggle(artwork);

        _favourites.Toggle(artwork);

        Assert.Empty(_favourites.GetAll());
        Assert.False(_favourites.IsFavourite(artwork));
        Assert.False(artwork.Starred);
    }

    [Fact]
    public void IsFavourite_SameArtworkFromNewSearch_ReturnsTrue()
    {
        _favourites.Toggle(CreateArtwork("111628"));

        Artwork fromNewSearch = CreateArtwork("111628");

        Assert.True(_favourites.IsFavourite(fromNewSearch));
    }

    [Fact]
    public void IsFavourite_SameIdFromDifferentMuseum_ReturnsFalse()
    {
        _favourites.Toggle(CreateArtwork("42", MuseumEnum.ArtInstituteOfChicago));

        Artwork otherMuseum = CreateArtwork("42", MuseumEnum.MetropolitanMuseumOfArt);

        Assert.False(_favourites.IsFavourite(otherMuseum));
    }

    private static Artwork CreateArtwork(string sourceId, MuseumEnum museum = MuseumEnum.ArtInstituteOfChicago)
    {
        return new Artwork(museum, sourceId, "Nighthawks", "Edward Hopper", 1942, "Oil on canvas", string.Empty, "Nighthawks");
    }
}
