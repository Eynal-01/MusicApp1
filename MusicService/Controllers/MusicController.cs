using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MusicService.Models;
using MusicService.Services.Abstract;

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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMusicById(string id)
        {
            var music = await _musicService.GetMusicByIdAsync(id);
            if (music == null) return NotFound();
            return Ok(music);
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
