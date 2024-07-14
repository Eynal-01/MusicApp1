////using Microsoft.AspNetCore.Http;
////using Microsoft.AspNetCore.Mvc;
////using MusicService.Models;
////using MusicService.Services.Abstract;
////using System.Linq;
////using System.Security.Claims;
////using System.Threading.Tasks;

////namespace MusicService.Controllers
////{
////    [Route("api/[controller]")]
////    [ApiController]
////    public class MusicController : ControllerBase
////    {
////        private readonly IMusicService _musicService;

////        public MusicController(IMusicService musicService)
////        {
////            _musicService = musicService;
////        }

////        [HttpGet("getallmusic")]
////        public async Task<IActionResult> GetAllMusic()
////        {
////            var musicList = await _musicService.GetAllMusicAsync();
////            return Ok(musicList);
////        }

////        [HttpGet("{id}")]
////        public async Task<IActionResult> GetMusicById(string userId)
////        {
////            IdentityService.Controllers.IdentityController.
////            var music = await _musicService.GetMusicByIdAsync(userId);
////            if (music == null) return NotFound();
////            return Ok(music);
////        }

////        [HttpGet("user/{userId}")]
////        //public async Task<IActionResult> GetMusicByUser(string userId)
////        //{
////        //    var musicList = await _musicService.GetMusicByUserIdAsync(userId);
////        //    return Ok(musicList);
////        //}

////        [HttpPost("AddMusic")]
////        public async Task<IActionResult> AddMusic([FromForm] Music music, [FromForm] IFormFile musicFile)
////        {
////            await _musicService.AddMusicAsync(music);
////            return CreatedAtAction(nameof(GetMusicById), new { id = music.Id }, music);
////        }

////        [HttpPut("{id}")]
////        public async Task<IActionResult> UpdateMusic(string id, [FromBody] Music music)
////        {
////            music.Id = id;
////            await _musicService.UpdateMusicAsync(music);
////            return NoContent();
////        }

////        [HttpDelete("{id}")]
////        public async Task<IActionResult> DeleteMusic(string id)
////        {
////            await _musicService.DeleteMusicAsync(id);
////            return NoContent();
////        }

////        [HttpPost("{id}/like")]
////        public async Task<IActionResult> IncrementLikeCount(string id)
////        {
////            await _musicService.IncrementLikeCountAsync(id);
////            return NoContent();
////        }
////    }
////}








using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicService.Models;
using MusicService.Services.Abstract;
using System.Security.Claims;
using System.Linq;

namespace MusicService.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;
        private readonly HttpClient _httpClient;

        public MusicController(IMusicService musicService, IHttpClientFactory httpClientFactory)
        {
            _musicService = musicService;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("getallmusic")]
        public async Task<IActionResult> GetAllMusic()
        {
            var musicList = await _musicService.GetAllMusicAsync();
            return Ok(musicList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusicById(string userId)
        {
            var music = await _musicService.GetMusicByIdAsync(userId);
            if (music == null) return NotFound();
            return Ok(music);
        }

        //[HttpGet]
        //public async Task<IActionResult> GetMusicById(string userId)
        //{
        //    // Get the userId from the token claims
        //    //var userId1 = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

        //    //if (string.IsNullOrEmpty(userId1))
        //    //{
        //    //    return Unauthorized();
        //    //}

        //    // Call the IdentityService to validate the user
        //    var response = await _httpClient.GetAsync($"https://localhost:7287/api/identity/validate?userId={userId}");

        //    if (!response.IsSuccessStatusCode)
        //    {
        //        return Unauthorized();
        //    }

        //    var music = await _musicService.GetMusicByIdAsync(userId);
        //    if (music == null) return NotFound();
        //    return Ok(music);
        //}

        [HttpGet("getmusicbyid")]
        public async Task<IActionResult> GetMusicByUser(string userId)
        {
            var musicList = await _musicService.GetAllMusicAsync();
            var filteredMusicList = musicList.Where(music => music.UserId == userId).ToList();
            return Ok(filteredMusicList);
        }


        [HttpPost("AddMusic")]
        public async Task<IActionResult> AddMusic([FromForm] Music music, [FromForm] IFormFile musicFile)
        {
            await _musicService.AddMusicAsync(music);
            return CreatedAtAction(nameof(GetMusicById), new { id = music.Id }, music);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMusic(string id, [FromBody] Music music)
        {
            music.Id = id;
            await _musicService.UpdateMusicAsync(music);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMusic(string id)
        {
            await _musicService.DeleteMusicAsync(id);
            return NoContent();
        }

        [HttpPost("{id}/like")]
        public async Task<IActionResult> IncrementLikeCount(string id)
        {
            await _musicService.IncrementLikeCountAsync(id);
            return NoContent();
        }
    }
}
























//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using MusicService.Models;
//using MusicService.Services.Abstract;
//using System.Linq;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace MusicService.Controllers
//{
//    //[Authorize]
//    [Route("api/[controller]")]
//    [ApiController]
//    public class MusicController : ControllerBase
//    {
//        private readonly IMusicService _musicService;
//        private readonly HttpClient _httpClient;

//        public MusicController(IMusicService musicService, IHttpClientFactory httpClientFactory)
//        {
//            _musicService = musicService;
//            _httpClient = httpClientFactory.CreateClient();
//        }

//        [HttpGet("getallmusic")]
//        public async Task<IActionResult> GetAllMusic()
//        {
//            var musicList = await _musicService.GetAllMusicAsync();
//            return Ok(musicList);
//        }

//        [HttpGet("{id}")]
//        public async Task<IActionResult> GetMusicById(string id)
//        {
//            var music = await _musicService.GetMusicByIdAsync(id);
//            if (music == null) return NotFound();
//            return Ok(music);
//        }

//        [HttpGet("user/{userId}")]
//        public async Task<IActionResult> GetMusicByUser(string userId)
//        {
//            var musicList = await _musicService.GetAllMusicAsync();
//            var filteredMusicList = musicList.Where(music => music.UserId == userId).ToList();
//            return Ok(filteredMusicList);
//        }

//        [HttpPost("AddMusic")]
//        public async Task<IActionResult> AddMusic([FromForm] Music music, [FromForm] IFormFile musicFile)
//        {
//            if (musicFile == null || musicFile.Length == 0)
//            {
//                return BadRequest("Music file is required.");
//            }

//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            /*music.FileName = musicFile.FileName;*/ // Assuming you want to store the filename in your model

//            await _musicService.AddMusicAsync(music);
//            return CreatedAtAction(nameof(GetMusicById), new { id = music.Id }, music);
//        }

//        [HttpPut("{id}")]
//        public async Task<IActionResult> UpdateMusic(string id, [FromBody] Music music)
//        {
//            if (!ModelState.IsValid)
//            {
//                return BadRequest(ModelState);
//            }

//            music.Id = id;
//            await _musicService.UpdateMusicAsync(music);
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteMusic(string id)
//        {
//            await _musicService.DeleteMusicAsync(id);
//            return NoContent();
//        }

//        [HttpPost("{id}/like")]
//        public async Task<IActionResult> IncrementLikeCount(string id)
//        {
//            await _musicService.IncrementLikeCountAsync(id);
//            return NoContent();
//        }
//    }
//}
