using MusicService.Models;
using MusicService.Services.Abstract;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace MusicService.Services.Concrete
{
    public class MusicService : IMusicService
    {
        static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis-15545.c9.us-east-1-4.ec2.redns.redis-cloud.com:15545,password=PiIMxwEexnZiw5Ta3NQW5YCfkYCNEWYZ");
        private readonly IDatabase _db;

        public MusicService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();
        }

        public async Task<IEnumerable<Music>> GetAllMusicAsync()
        {
            var keys = redis.GetServer(redis.GetEndPoints().First()).Keys(pattern: "music:*");
            var musicList = new List<Music>();

            foreach (var key in keys)
            {
                var musicData = await _db.StringGetAsync(key);
                if (musicData.HasValue)
                {
                    musicList.Add(JsonConvert.DeserializeObject<Music>(musicData));
                }
            }
            return musicList;
        }

        public async Task<Music> GetMusicByIdAsync(string id)
        {
            var musicData = await _db.StringGetAsync($"music:{id}");
            return musicData.HasValue ? JsonConvert.DeserializeObject<Music>(musicData) : null;
        }

        public async Task AddMusicAsync(Music music)
        {
            music.Id = Guid.NewGuid().ToString();
            var musicData = JsonConvert.SerializeObject(music);
            await _db.StringSetAsync($"music:{music.Id}", musicData);
        }

        public async Task UpdateMusicAsync(Music music)
        {
            var musicData = JsonConvert.SerializeObject(music);
            await _db.StringSetAsync($"music:{music.Id}", musicData);
        }

        public async Task DeleteMusicAsync(string id)
        {
            await _db.KeyDeleteAsync($"music:{id}");
        }

        public async Task IncrementLikeCountAsync(string id)
        {
            var musicData = await _db.StringGetAsync($"music:{id}");
            if (musicData.HasValue)
            {
                var music = JsonConvert.DeserializeObject<Music>(musicData);
                music.LikeCount++;
                await _db.StringSetAsync($"music:{music.Id}", JsonConvert.SerializeObject(music));
            }
        }
    }
}
