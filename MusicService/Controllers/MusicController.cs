using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Models;
using MusicService.Services.Abstract;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicController : ControllerBase
    {
        private readonly IMusicService _musicService;

        public MusicController(IMusicService musicService)
        {
            _musicService = musicService;
        }

        [HttpGet("getallmusic")]
        public async Task<IActionResult> GetAllMusic()
        {
            var musicList = await _musicService.GetAllMusicAsync();
            return Ok(musicList);
        }


        [HttpGet("getmusicbyid")]
        public async Task<IActionResult> GetMusicByUser(string userId)
        {
            var musicList = await _musicService.GetAllMusicAsync();
            var filteredMusicList = musicList.Where(music => music.UserId == userId).ToList();
            return Ok(filteredMusicList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusicById(string userId)
        {
            var music = await _musicService.GetMusicByIdAsync(userId);
            if (music == null) return NotFound();
            return Ok(music);
        }

        //[HttpGet("user/{userId}")]
        //public async Task<IActionResult> GetMusicByUser(string userId)
        //{
        //    var musicList = await _musicService.GetMusicByUserIdAsync(userId);
        //    return Ok(musicList);
        //}

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

        //[HttpDelete("deletemusic")]
        //public async Task<IActionResult> DeleteMusic(string id)
        //{
        //    await _musicService.DeleteMusicAsync(id);
        //    return NoContent();
        //}

        [HttpDelete("deletemusic")]
        public async Task<IActionResult> DeleteMusic(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Invalid music id");
            }

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