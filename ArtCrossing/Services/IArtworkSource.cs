using ArtCrossing.Models;

namespace ArtCrossing.Services;

public interface IArtworkSource
{
    MuseumEnum Museum { get; }

    Task<IReadOnlyList<Artwork>> SearchAsync(string query, CancellationToken cancellationToken = default);
}
