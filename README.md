# Santander Hacker News Api

I was asked to take no more than a hour to finish up this task. I took a bit more than an hour. Ideally before shipping to Prod I would implement the enhancements detailed below.

## How to build and run:
* Clone the repository
* Open a command window
* In the commnd window 
  - `cd` to `Santander\SantanderHackerNewsApi`
  - dotnet build
  - dotnet run
* Open swagger in a web browser by following the link: [https://localhost:7065/swagger/index.html][https://localhost:7065/swagger/index.html]

## Assumptions:
I have assumed the data at HackerNews doesn't change very frequently. Hence the cache I implemented remains valid for an hour, allowing us to reduce the number of requests we make to HackerNews.
This cache refresh time would need to be updated accordingly based on further research on how frequently it changes.

## Enhancements:
* I would use interfaces for all dependencies (for example `HttpClient`) to allow for mocking and testing.
* I would move all the code in `Program.cs` to `Startup.cs`. That is a cleaner way to structure the project.
* I have hardcoded values for a few settings (Urls, cache timeout). These should go into the `appsettings.*.json`
* It would be good to implement proper checks for
  - types other than int
  - null conditions
* I would have loved to write unit tests by mocking `HttpClient` to ensure all code paths are tested.
