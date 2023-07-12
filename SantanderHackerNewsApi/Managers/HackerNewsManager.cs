using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SantanderHackerNewsApi.Models;

namespace SantanderHackerNewsApi.Managers;

public class HackerNewsManager
{
    private const string bestStoriesUri = "https://hacker-news.firebaseio.com/v0/beststories.json";
    private const string storyUri = "https://hacker-news.firebaseio.com/v0/item/{0}.json";
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _memCache;


    public HackerNewsManager(HttpClient httpClient, IMemoryCache memCache)
    {
        _httpClient = httpClient;
        _memCache = memCache;
    }

    public async Task<IEnumerable<Story?>> GetTopNStories(int topN)
    {
        if (topN < 1 || topN > 200)
        {
            throw new ArgumentOutOfRangeException($"{nameof(topN)} should be between 1 and 200");
        }

        var topNStoryIds = await GetTopNStoryIds(topN);

        var storiesTaskList = new List<Task<Story?>>();
        foreach (var storyId in topNStoryIds)
        {
            storiesTaskList.Add(Task.Run<Story?>(() => GetBestStory(storyId)));
        }

        await Task.WhenAll(storiesTaskList);

        return storiesTaskList.Select(x => x.Result).OrderByDescending(x => x.Score).ToArray();
    }

    private async Task<List<int>?> GetTopNStoryIds(int topN)
    {
        var resultsBestStories = await _httpClient.GetAsync(bestStoriesUri);

        if (!resultsBestStories.IsSuccessStatusCode)
        {
            throw new Exception($"Could not fetch best story Ids. StatusCode: {resultsBestStories.StatusCode}, ReasonPhrase: {resultsBestStories.ReasonPhrase}");
        }

        var response = await resultsBestStories.Content.ReadAsStringAsync();
        var storiesIdArray = JsonConvert.DeserializeObject<List<int>>(response);

        //TODO: may require a check here in case storiesIdArray is blank
        return storiesIdArray?.Take(topN).ToList();
    }

    private async Task<Story?> GetBestStory(int storyId)
    {
        return await _memCache.GetOrCreateAsync(storyId, async cacheEntry =>
        {
            cacheEntry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            var uriToUse = string.Format(storyUri, storyId);
            var resultsStory = await _httpClient.GetAsync(uriToUse);

            if (!resultsStory.IsSuccessStatusCode)
            {
                throw new Exception($"Could not fetch best stories. StatusCode: {resultsStory.StatusCode}, ReasonPhrase: {resultsStory.ReasonPhrase}");
            }

            var response = await resultsStory.Content.ReadAsStringAsync();
            var story = JsonConvert.DeserializeObject<Story>(response);
            return story;
        });
    }
}
