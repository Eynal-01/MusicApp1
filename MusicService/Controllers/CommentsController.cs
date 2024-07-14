using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Models;
using Newtonsoft.Json;

namespace MusicService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly RabbitMQHelper _rabbitMQHelper;

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

            comment.CreatedAt = DateTime.UtcNow;
            var commentJson = JsonConvert.SerializeObject(comment);
            _rabbitMQHelper.Publish(commentJson);

            return Ok(comment);
        }
    }
}