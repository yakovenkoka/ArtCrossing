namespace ArtCrossing.Models;

/// <summary>
/// The museums whose collections Art Crossing searches. Each one has its own
/// artwork source that talks to that museum's API.
/// </summary>
public enum MuseumEnum
{
    Undefined,
    ArtInstituteOfChicago,
    MetropolitanMuseumOfArt
}
