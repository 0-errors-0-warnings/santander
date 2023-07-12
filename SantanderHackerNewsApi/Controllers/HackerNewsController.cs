using Microsoft.AspNetCore.Mvc;
using SantanderHackerNewsApi.Managers;
using SantanderHackerNewsApi.Models;

namespace SantanderHackerNewsApi.Controllers;

[ApiController]
[Route("[controller]")]
public class HackerNewsController : ControllerBase
{
    private readonly ILogger<HackerNewsController> _logger;
    private HackerNewsManager _manager;

    public HackerNewsController(ILogger<HackerNewsController> logger, HackerNewsManager manager)
    {
        _logger = logger;
        _manager = manager;
    }

    //[HttpGet(Name = "GetTopNStories")]
    [HttpGet, Route("{topN}")]
    public async Task<IEnumerable<Story?>> GetTopNStoriesAsync(int topN) => await _manager.GetTopNStories(topN);
}

