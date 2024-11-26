using HackerNewsAPI.Models;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace HackerNewsAPI.Services;

public class HackerNewsService(HttpClient httpClient, IMemoryCache cache)
{
    private const string BestStoriesUrl = "https://hacker-news.firebaseio.com/v0/beststories.json";
    private const string StoryDetailUrl = "https://hacker-news.firebaseio.com/v0/item/{0}.json";

    public async Task<List<Story>> GetTopStoriesAsync(int count)
    {
        var bestStoryIds = await GetBestStoryIdsAsync();
        var stories = new List<Story>();

        if (bestStoryIds == null) return stories.Where(s => true).OrderByDescending(s => s.Score).ToList();
        foreach (var batch in bestStoryIds.Take(count).Chunk(10))
        {
            var tasks = batch.Select(GetStoryDetailsAsync);
            stories.AddRange((await Task.WhenAll(tasks))!);
        }

        return stories.Where(s => true).OrderByDescending(s => s.Score).ToList();
    }


    private async Task<List<int>?> GetBestStoryIdsAsync()
    {
        if (cache.TryGetValue("BestStories", out List<int>? cachedIds)) return cachedIds;

        var response = await httpClient.GetStringAsync(BestStoriesUrl);
        var ids = JsonConvert.DeserializeObject<List<int>>(response);

        cache.Set("BestStories", ids, TimeSpan.FromMinutes(5));
        return ids;
    }

    private async Task<Story?> GetStoryDetailsAsync(int id)
    {
        if (cache.TryGetValue($"Story_{id}", out Story? cachedStory)) return cachedStory;

        var url = string.Format(StoryDetailUrl, id);
        try
        {
            var response = await httpClient.GetStringAsync(url);
            var storyData = JsonConvert.DeserializeObject<dynamic>(response);

            var story = new Story
            {
                Title = storyData?.title,
                Uri = storyData?.url ?? "",
                PostedBy = storyData?.by,
                Time = DateTimeOffset.FromUnixTimeSeconds((long) (storyData?.time ?? throw new InvalidOperationException())).UtcDateTime,
                Score = storyData.score ?? 0,
                CommentCount = storyData.descendants ?? 0
            };

            cache.Set($"Story_{id}", story, TimeSpan.FromMinutes(10));
            return story;
        }
        catch
        {
            return null;
        }
    }
}