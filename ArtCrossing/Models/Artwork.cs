namespace ArtCrossing.Models;

public record Artwork : IComparable<Artwork>
{
    private MuseumEnum _museum;
    private string _sourceId;
    private string _title;
    private string _artist;
    private int? _completionYear;
    private string _medium;
    private string _imageUrl;
    private string _altText;
    private bool _starred;

    public MuseumEnum Museum
    {
        get => _museum;
        set => _museum = value;
    }
    
    public string SourceId
    {
        get => _sourceId;
        set => _sourceId = value;
    }
    
    public string Title
    {
        get => _title;
        set => _title = value;
    }
    
    public string Artist
    {
        get => _artist;
        set => _artist = value;
    }
    
    public int? CompletionYear
    {
        get => _completionYear;
        set => _completionYear = value;
    }
    
    public string Medium
    {
        get => _medium;
        set => _medium = value;
    }
    
    public string ImageUrl
    {
        get => _imageUrl;
        set => _imageUrl = value;
    }
    
    public string AltText
    {
        get => _altText;
        set => _altText = value;
    }
    
    public bool Starred
    {
        get => _starred;
        set => _starred = value;
    }

    public Artwork()
    {
        _museum = MuseumEnum.Undefined;
        _sourceId = string.Empty;
        _title = string.Empty;
        _artist = string.Empty;
        _completionYear = null;
        _medium = string.Empty;
        _imageUrl = string.Empty;
        _altText = string.Empty;
        _starred = false;
    }

    public Artwork(MuseumEnum museum, string sourceId, string title, string artist, int? completionYear, string medium,
        string url, string altText)
    {
        _museum = museum;
        _sourceId = sourceId;
        _title = title;
        _artist = artist;
        _completionYear = completionYear;
        _medium = medium;
        _imageUrl = url;
        _altText = altText;
        _starred = false;
    }
    
    public string MuseumName => Museum switch
    {
        MuseumEnum.ArtInstituteOfChicago => "Art Institute of Chicago",
        MuseumEnum.MetropolitanMuseumOfArt => "The Met",
        _ => "Unknown museum"
    };

    public string WebUrl => Museum switch
    {
        MuseumEnum.ArtInstituteOfChicago => $"https://www.artic.edu/artworks/{SourceId}",
        MuseumEnum.MetropolitanMuseumOfArt => $"https://www.metmuseum.org/art/collection/search/{SourceId}",
        _ => string.Empty
    };
    
    public string Key => $"{Museum}-{SourceId}";
    
    public int CompareTo(Artwork? other)
    {
        if (other is null)
        {
            return 1;
        }
        if (CompletionYear is null && other.CompletionYear is not null)
        {
            return 1;
        }
        if (other.CompletionYear is null && CompletionYear is not null)
        {
            return -1;
        }
        int byYear = Nullable.Compare(CompletionYear, other.CompletionYear);
        if (byYear != 0)
        {
            return byYear;
        }
        
        return string.Compare(Title, other.Title, StringComparison.CurrentCultureIgnoreCase);
    }

    public override string ToString()
    {
        return $"[{Title} : {CompletionYear} : {Artist}]";
    }

}
