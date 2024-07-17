////using Microsoft.AspNetCore.Http;
////using Microsoft.AspNetCore.Mvc;
////using MusicService.Models;
////using Newtonsoft.Json;

////namespace MusicService.Controllers
////{
////    [ApiController]
////    [Route("api/[controller]")]
////    public class CommentsController : ControllerBase
////    {
////        private readonly RabbitMQHelper _rabbitMQHelper;

////        public CommentsController()
////        {
////            _rabbitMQHelper = new RabbitMQHelper();
////        }

////        [HttpPost("addcomment")]
////        public IActionResult AddComment([FromBody] Comment comment)
////        {
////            if (comment == null)
////            {
////                return BadRequest("Comment is null.");
////            }

////            comment.CreatedAt = DateTime.UtcNow;
////            var commentJson = JsonConvert.SerializeObject(comment);
////            _rabbitMQHelper.Publish(commentJson);

////            return Ok(comment);
////        }

////        //[HttpGet("getcomments")]
////        //public IActionResult GetComments(int musicId)
////        //{
////        //    // This should interact with your data source to fetch comments.
////        //    // Since this example doesn't include a data layer, here's a placeholder implementation.
////        //    var comments = new List<Comment>
////        //    {
////        //        new Comment { Id = 1, MusicId = musicId, UserId = "user1", Content = "Great song!", CreatedAt = DateTime.UtcNow }
////        //        // Add more sample comments if needed.
////        //    };

////        //    return Ok(comments);
////        //}
////    }
////}






















//using Microsoft.AspNetCore.Mvc;
//using MusicService.Models;
//using Newtonsoft.Json;

//namespace MusicService.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class CommentsController : ControllerBase
//    {
//        private readonly RabbitMQHelper _rabbitMQHelper;

//        public CommentsController()
//        {
//            _rabbitMQHelper = new RabbitMQHelper();
//        }

//        [HttpPost("addcomment")]
//        public IActionResult AddComment([FromBody] Comment comment)
//        {
//            if (comment == null)
//            {
//                return BadRequest("Comment is null.");
//            }

//            comment.CreatedAt = DateTime.UtcNow;
//            var commentJson = JsonConvert.SerializeObject(comment);
//            _rabbitMQHelper.Publish(commentJson);

//            return Ok(comment);
//        }

//        [HttpGet("getcomments")]
//        public IActionResult GetComments(int musicId)
//        {
//            // Retrieve comments from your data source here
//            // This is just a placeholder example
//            var comments = new List<Comment>
//            {
//                new Comment { Id = 1, MusicId = musicId, UserId = "user1", Content = "Great song!", CreatedAt = DateTime.UtcNow }
//            };

//            return Ok(comments);
//        }
//    }
//}






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

        [HttpGet("getcomments")]
        public IActionResult GetComments(int musicId)
        {
            // Retrieve comments from your data source here
            // This is just a placeholder example
            var comments = new List<Comment>
            {
                new Comment { Id = 1, MusicId = musicId, UserId = "user1", Content = "Great song!", CreatedAt = DateTime.UtcNow }
            };

            return Ok(comments);
        }
    }
}

