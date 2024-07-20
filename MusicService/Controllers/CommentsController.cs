using Microsoft.AspNetCore.Mvc;
using MusicService.Models;
using Newtonsoft.Json;

[ApiController]
[Route("api/[controller]")]
public class CommentsController : ControllerBase
{
    private readonly RabbitMQHelper _rabbitMQHelper;
    private static List<Comment> _commentsStore = new List<Comment>();

    public CommentsController()
    {
        _rabbitMQHelper = new RabbitMQHelper();
    }
    [HttpPost("addcomment")]
    public IActionResult AddComment([FromBody] Comment comment)
    {
        if (comment == null)
        {
            return BadRequest("Comment is null.");
        }

        if (string.IsNullOrEmpty(comment.Content))
        {
            return BadRequest("Comment content is required.");
        }

        if (comment.MusicId == Guid.Empty)
        {
            return BadRequest("MusicId is required and must be a valid GUID.");
        }

        comment.CreatedAt = DateTime.UtcNow;
        var commentJson = JsonConvert.SerializeObject(comment);
        _rabbitMQHelper.Publish(commentJson);

        return Ok(comment);
    }

    [HttpGet("getcomments/{musicId}")]
    public IActionResult GetComments(string musicId)
    {
        if (string.IsNullOrEmpty(musicId) || !Guid.TryParse(musicId, out Guid musicGuid))
        {
            return BadRequest("MusicId is required and must be a valid GUID.");
        }

        // Retrieve comments from the in-memory store
        var comments = _commentsStore.Where(c => c.MusicId == musicGuid).ToList();

        if (comments == null || comments.Count == 0)
        {
            return NotFound("No comments found for the provided MusicId.");
        }

        return Ok(comments);
    }



}
