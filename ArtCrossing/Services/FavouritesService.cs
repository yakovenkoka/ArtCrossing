using ArtCrossing.Models;

namespace ArtCrossing.Services;

public class FavouritesService
{
    private readonly Dictionary<string, Artwork> _favourites = new();

    public IReadOnlyList<Artwork> GetAll()
    {
        return _favourites.Values.ToList();
    }

    public bool IsFavourite(Artwork artwork)
    {
        return _favourites.ContainsKey(artwork.Key);
    }

    public void Toggle(Artwork artwork)
    {
        if (_favourites.Remove(artwork.Key))
        {
            artwork.Starred = false;
        }
        else
        {
            _favourites.Add(artwork.Key, artwork);
            artwork.Starred = true;
        }
    }
}
