# Art Crossing

Art Crossing is a Blazor web app for exploring artworks from two major museum collections in one place. A single search queries both the Art Institute of Chicago and The Metropolitan Museum of Art, so visitors, students and art lovers can compare works across collections without switching between museum websites.

## Features

- Search artworks across the Art Institute of Chicago and The Met at the same time
- Artwork cards with image, title, artist, date and medium, linking to the artwork's page on the museum website
- Favourites page to collect artworks of interest by starring them

## Technologies

- .NET 10 and ASP.NET Core Blazor
- Bootstrap 5.3 and scoped CSS for styling
- xUnit for unit tests

## Project structure

- `ArtCrossing` - the Blazor web app
  - `Components` - pages, layout and the artwork card
  - `Models` - the shared `Artwork` model
  - `Services` - one `IArtworkSource` implementation per museum, and the favourites service
- `ArtCrossing.Tests` - xUnit tests for the model, services and API sources, using a fake `HttpMessageHandler` so no real network calls are made

## Issues
-  Art Institute of Chicago images are blocked by Cloudflare's bot protection when loaded from outside artic.edu, even though API docs does not mention that. 

## Getting started


Run the app:

```
dotnet run --project ArtCrossing
```

Then open http://localhost:5088.

Run the tests:

```
dotnet test
```

## References

### Web services

- [Art Institute of Chicago API](https://api.artic.edu/docs/)
- [Art Institute of Chicago IIIF Image API](https://api.artic.edu/docs/#iiif-image-api)
- [The Metropolitan Museum of Art Collection API](https://metmuseum.github.io/)

### .NET and Blazor

- [Blazor render modes](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/render-modes?view=aspnetcore-10.0)
- [Blazor data binding](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/data-binding?view=aspnetcore-10.0)
- [Blazor event handling](https://learn.microsoft.com/en-us/aspnet/core/blazor/components/event-handling?view=aspnetcore-10.0)
- [Blazor dependency injection](https://learn.microsoft.com/en-us/aspnet/core/blazor/fundamentals/dependency-injection?view=aspnetcore-10.0)

### HTTP and JSON

- [Call a web API from Blazor](https://learn.microsoft.com/en-us/aspnet/core/blazor/call-web-api?view=aspnetcore-10.0)
- [IHttpClientFactory and typed clients](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-10.0)
- [HttpClientJsonExtensions.GetFromJsonAsync](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.json.httpclientjsonextensions.getfromjsonasync)
- [System.Text.Json overview](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/overview)
- [Configure System.Text.Json options](https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/configure-options)

### C#

- [Records](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/record)
- [Create user-defined exceptions](https://learn.microsoft.com/en-us/dotnet/standard/exceptions/how-to-create-user-defined-exceptions)
- [Asynchronous programming with async and await](https://learn.microsoft.com/en-us/dotnet/csharp/asynchronous-programming/)
- [Task.WhenAll](https://learn.microsoft.com/en-us/dotnet/api/system.threading.tasks.task.whenall)
- [Cancellation in managed threads](https://learn.microsoft.com/en-us/dotnet/standard/threading/cancellation-in-managed-threads)

### Testing

- [Unit testing C# with xUnit](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-csharp-with-xunit)
- [Unit testing best practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- [xUnit v2 getting started](https://xunit.net/docs/getting-started/v2/getting-started)
- [HttpMessageHandler](https://learn.microsoft.com/en-us/dotnet/api/system.net.http.httpmessagehandler)
- [Coverlet](https://github.com/coverlet-coverage/coverlet)

### UI

- [Playfair Display font](https://fonts.google.com/specimen/Playfair+Display)
- [Material Symbols icons](https://fonts.google.com/icons)
