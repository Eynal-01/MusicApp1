//using MusicService.Models;

//namespace MusicService.Services.Abstract
//{
//    public interface IMusicService
//    {
//        Task<IEnumerable<Music>> GetAllMusicAsync();
//        Task<Music> GetMusicByIdAsync(string id);
//        Task AddMusicAsync(Music music);
//        Task UpdateMusicAsync(Music music);
//        Task DeleteMusicAsync(string id);
//        Task IncrementLikeCountAsync(string id);
//    }
//}





using MusicService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MusicService.Services.Abstract
{
    //public interface IMusicService
    //{
    //    Task<IEnumerable<Music>> GetAllMusicAsync();
    //    Task<Music> GetMusicByIdAsync(string id);
    //    Task<IEnumerable<Music>> GetMusicByUserIdAsync(string userId); // Add this method
    //    Task AddMusicAsync(Music music, IFormFile musicFile);
    //    Task UpdateMusicAsync(Music music);
    //    Task DeleteMusicAsync(string id);
    //    Task IncrementLikeCountAsync(string id);
    //}

    public interface IMusicService
    {
        Task<IEnumerable<Music>> GetAllMusicAsync();
        Task<Music> GetMusicByIdAsync(string id);
        Task AddMusicAsync(Music music);
        Task UpdateMusicAsync(Music music);
        Task DeleteMusicAsync(string id);
        Task IncrementLikeCountAsync(string id);
    }
}
