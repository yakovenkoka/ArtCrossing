using ArtCrossing.Models;

namespace ArtCrossing.Services;

public class ArtworkSourceException : Exception
{
    public MuseumEnum Museum { get; }

    public ArtworkSourceException(MuseumEnum museum, string message, Exception innerException)
        : base(message, innerException)
    {
        Museum = museum;
    }
}
