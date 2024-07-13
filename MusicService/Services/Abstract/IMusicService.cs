using MusicService.Models;

namespace MusicService.Services.Abstract
{
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
