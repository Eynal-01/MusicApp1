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

        //[HttpPost("AddMusic")]
        //public async Task<IActionResult> AddMusic([FromForm] Music music, [FromForm] IFormFile musicFile)
        //{
        //    await _musicService.AddMusicAsync(music);
        //    return CreatedAtAction(nameof(GetMusicById), new { id = music.Id }, music);
        //}

        //[HttpPost("AddMusic")]
        //public async Task<IActionResult> AddMusic([FromForm] Music music, [FromForm] IFormFile musicFile, [FromForm] IFormFile imageFile)
        //{
        //    if (musicFile != null && imageFile != null)
        //    {
        //        var musicFilePath = Path.Combine("MusicFiles", Guid.NewGuid().ToString() + Path.GetExtension(musicFile.FileName));
        //        var imageFilePath = Path.Combine("MusicImages", Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName));

        //        using (var stream = new FileStream(musicFilePath, FileMode.Create))
        //        {
        //            await musicFile.CopyToAsync(stream);
        //        }

        //        using (var stream = new FileStream(imageFilePath, FileMode.Create))
        //        {
        //            await imageFile.CopyToAsync(stream);
        //        }

        //        //music.FilePath = musicFilePath;
        //        music.ImagePath = imageFilePath;
        //    }

        //    await _musicService.AddMusicAsync(music);
        //    return CreatedAtAction(nameof(GetMusicById), new { id = music.Id }, music);
        //}




        //[HttpPost("AddMusic")]
        //public async Task<IActionResult> AddMusic([FromForm] Music music, [FromForm] IFormFile musicFile, [FromForm] IFormFile imageFile)
        //{
        //    if (musicFile == null || imageFile == null)
        //    {
        //        return BadRequest("Music file and image file are required.");
        //    }

        //    try
        //    {
        //        // Ensure the directories exist
        //        var musicDirectory = Path.Combine("MusicFiles");
        //        var imageDirectory = Path.Combine("MusicImages");

        //        if (!Directory.Exists(musicDirectory))
        //        {
        //            Directory.CreateDirectory(musicDirectory);
        //        }

        //        if (!Directory.Exists(imageDirectory))
        //        {
        //            Directory.CreateDirectory(imageDirectory);
        //        }

        //        var musicFilePath = Path.Combine(musicDirectory, Guid.NewGuid().ToString() + Path.GetExtension(musicFile.FileName));
        //        var imageFilePath = Path.Combine(imageDirectory, Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName));

        //        using (var musicStream = new FileStream(musicFilePath, FileMode.Create))
        //        {
        //            await musicFile.CopyToAsync(musicStream);
        //        }

        //        using (var imageStream = new FileStream(imageFilePath, FileMode.Create))
        //        {
        //            await imageFile.CopyToAsync(imageStream);
        //        }

        //        music.FilePath = musicFilePath;
        //        music.ImagePath = imageFilePath;

        //        await _musicService.AddMusicAsync(music);
        //        return CreatedAtAction(nameof(GetMusicById), new { id = music.Id }, music);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Log the exception
        //        // Example: _logger.LogError(ex, "An error occurred while adding music.");

        //        return StatusCode(500, $"Internal server error: {ex.Message}");
        //    }
        //}


        [HttpPost("AddMusic")]
        public async Task<IActionResult> AddMusic([FromForm] Music music, [FromForm] IFormFile musicFile, [FromForm] IFormFile imageFile)
        {
            if (musicFile == null || imageFile == null)
            {
                return BadRequest("Music file and image file are required.");
            }

            try
            {
                // Ensure the directories exist
                var musicDirectory = Path.Combine("MusicFiles");
                var imageDirectory = Path.Combine("MusicImages");

                if (!Directory.Exists(musicDirectory))
                {
                    Directory.CreateDirectory(musicDirectory);
                }

                if (!Directory.Exists(imageDirectory))
                {
                    Directory.CreateDirectory(imageDirectory);
                }

                var musicFilePath = Path.Combine(musicDirectory, Guid.NewGuid().ToString() + Path.GetExtension(musicFile.FileName));
                var imageFilePath = Path.Combine(imageDirectory, Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName));

                using (var musicStream = new FileStream(musicFilePath, FileMode.Create))
                {
                    await musicFile.CopyToAsync(musicStream);
                }

                using (var imageStream = new FileStream(imageFilePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(imageStream);
                }

                music.Id = Guid.NewGuid().ToString();
                music.MusicFile = null; // We don't need to store the file itself, just the path
                music.ImagePath = imageFilePath;

                await _musicService.AddMusicAsync(music);
                return CreatedAtAction(nameof(GetMusicById), new { id = music.Id }, music);
            }
            catch (Exception ex)
            {
                // Log the exception
                // Example: _logger.LogError(ex, "An error occurred while adding music.");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
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

        [HttpPost("{id}/dislike")]
        public async Task<IActionResult> DecrementLikeCount(string id)
        {
            await _musicService.DecrementLikeCountAsync(id);
            return NoContent();
        }

    }
}