//using MusicService.Models;
//using MusicService.Services.Abstract;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace MusicService.Services
//{
//    public class MusicService : IMusicService
//    {
//        private readonly List<Music> _musicList = new(); // Replace with your actual data store

//        public async Task<IEnumerable<Music>> GetAllMusicAsync()
//        {
//            return await Task.FromResult(_musicList);
//        }

//        public async Task<Music> GetMusicByIdAsync(string id)
//        {
//            var music = _musicList.FirstOrDefault(m => m.Id == id);
//            return await Task.FromResult(music);
//        }

//        public async Task<IEnumerable<Music>> GetMusicByUserIdAsync(string userId)
//        {
//            var musicList = _musicList.Where(m => m.UserId == userId);
//            return await Task.FromResult(musicList);
//        }

//        public async Task AddMusicAsync(Music music, IFormFile musicFile)
//        {
//            // Handle file upload logic here

//            _musicList.Add(music);
//            await Task.CompletedTask;
//        }

//        public async Task UpdateMusicAsync(Music music)
//        {
//            var existingMusic = _musicList.FirstOrDefault(m => m.Id == music.Id);
//            if (existingMusic != null)
//            {
//                existingMusic.Author = music.Author;
//                existingMusic.Title = music.Title;
//                existingMusic.Description = music.Description;
//                // Update other properties as needed
//            }
//            await Task.CompletedTask;
//        }

//        public async Task DeleteMusicAsync(string id)
//        {
//            var music = _musicList.FirstOrDefault(m => m.Id == id);
//            if (music != null)
//            {
//                _musicList.Remove(music);
//            }
//            await Task.CompletedTask;
//        }

//        public async Task IncrementLikeCountAsync(string id)
//        {
//            var music = _musicList.FirstOrDefault(m => m.Id == id);
//            if (music != null)
//            {
//                music.LikeCount++;
//            }
//            await Task.CompletedTask;
//        }
//    }
//}
