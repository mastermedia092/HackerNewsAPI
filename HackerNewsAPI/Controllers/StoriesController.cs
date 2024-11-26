using HackerNewsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoriesController(HackerNewsService service) : ControllerBase
{
    [HttpGet("top")]
    public async Task<IActionResult> GetTopStories([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        if (page <= 0 || pageSize <= 0) return BadRequest("Page and pageSize must be greater than 0.");

        var stories = await service.GetTopStoriesAsync(page * pageSize);
        var paginated = stories.Skip((page - 1) * pageSize).Take(pageSize);

        return Ok(new
        {
            totalStories = stories.Count,
            page,
            pageSize,
            stories = paginated
        });
    }
}