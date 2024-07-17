using MusicService.Services.Abstract;
using MusicService.Services;
using StackExchange.Redis;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MusicService.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting.Server;

namespace MusicService.Server
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHttpClient();
            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });



            // Configure JWT authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Authority = "https://localhost:7287"; // URL of your IdentityService
                options.Audience = "MusicService"; // The audience value you set in your IdentityService
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("supersecretkey123")) // Replace with your actual secret key
                };
            });

            // Register Redis connection and MusicService
            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect("redis-15545.c9.us-east-1-4.ec2.redns.redis-cloud.com:15545,password=PiIMxwEexnZiw5Ta3NQW5YCfkYCNEWYZ"));
            builder.Services.AddScoped<IMusicService, MusicService>();
            builder.Services.AddHostedService<RabbitMQConsumerService>();

            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseDefaultFiles();
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();

            // Apply the CORS policy
            app.UseCors("AllowReactApp");

            // Apply authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapFallbackToFile("/index.html");
            app.Run();
        }
    }


    public class MusicService : IMusicService
    {
        static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("redis-15545.c9.us-east-1-4.ec2.redns.redis-cloud.com:15545,password=PiIMxwEexnZiw5Ta3NQW5YCfkYCNEWYZ");
        private readonly IDatabase _db;

        public MusicService(IConnectionMultiplexer redis)
        {
            _db = redis.GetDatabase();

        }

        //public async Task<IEnumerable<Music>> GetAllMusicAsync()
        //{
        //    var keys = redis.GetServer(redis.GetEndPoints().First()).Keys(pattern: "music:*");
        //    var musicList = new List<Music>();

        //    foreach (var key in keys)
        //    {
        //        var musicData = await _db.StringGetAsync(key);
        //        if (musicData.HasValue)
        //        {
        //            musicList.Add(JsonConvert.DeserializeObject<Music>(musicData));
        //        }
        //    }
        //    return musicList;
        //}

        //public async Task<IEnumerable<Music>> GetAllMusicAsync()
        //{
        //    var keys = redis.GetServer(redis.GetEndPoints().First()).Keys(pattern: "music:*");
        //    var musicList = new List<Music>();

        //    foreach (var key in keys)
        //    {
        //        Console.WriteLine($"Key: {key}");
        //        var musicData = await _db.StringGetAsync(key);
        //        if (musicData.HasValue)
        //        {
        //            Console.WriteLine($"Music Data: {musicData}");
        //            try
        //            {
        //                var music = JsonConvert.DeserializeObject<Music>(musicData);
        //                if (music != null)
        //                {
        //                    musicList.Add(music);
        //                }
        //                else
        //                {
        //                    Console.WriteLine("Deserialized music is null.");
        //                }
        //            }
        //            catch (JsonSerializationException ex)
        //            {
        //                Console.WriteLine($"Error deserializing music data: {ex.Message}");
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No value for this key.");
        //        }
        //    }

        //    Console.WriteLine($"Total music items retrieved: {musicList.Count}");
        //    return musicList;
        //}



        //public async Task<IEnumerable<Music>> GetAllMusicAsync()
        //{
        //    var keys = redis.GetServer(redis.GetEndPoints().First()).Keys(pattern: "music:*");
        //    var musicList = new List<Music>();

        //    foreach (var key in keys)
        //    {
        //        var musicData = await _db.StringGetAsync(key);
        //        if (musicData.HasValue)
        //        {
        //            musicList.Add(JsonConvert.DeserializeObject<Music>(musicData));
        //        }
        //    }
        //    return musicList;
        //}



        //public async Task<IEnumerable<Music>> GetAllMusicAsync()
        //{
        //    var keys = redis.GetServer(redis.GetEndPoints().First()).Keys(pattern: "music:*");
        //    var musicList = new List<Music>(); 

        //    foreach(var key in keys)
        //    {
        //        var musicData = await _db.StringGetAsync(key);
        //        if (musicData.HasValue)
        //        {
        //            var music = JsonConvert.DeserializeObject<Music>(musicData.ToString()); // Convert RedisValue to string
        //            musicList.Add(music);
        //        }
        //    }
        //    return musicList;
        //}


        public async Task<IEnumerable<Music>> GetAllMusicAsync()
        {
            var keys = redis.GetServer(redis.GetEndPoints().First()).Keys(pattern: "music:*");
            var musicList = new List<Music>();

            foreach (var key in keys)
            {
                var musicData = await _db.StringGetAsync(key);
                if (musicData.HasValue)
                {
                    try
                    {
                        var music = JsonConvert.DeserializeObject<Music>(musicData.ToString());
                        if (music != null && IsValidMusic(music))
                        {
                            musicList.Add(music);
                        }
                    }
                    catch (JsonException ex)
                    {
                        // Log the exception or handle it as needed
                        Console.WriteLine($"Error deserializing key {key}: {ex.Message}");
                    }
                }
            }

            return musicList;
        }

        private bool IsValidMusic(Music music)
        {
            // Perform necessary validation to ensure the music object has valid properties
            // Example validation:
            return !string.IsNullOrEmpty(music.Title) && !string.IsNullOrEmpty(music.Author);
        }

        public async Task<Music> GetMusicByIdAsync(string id)
        {
            var musicData = await _db.StringGetAsync($"music:{id}");
            return musicData.HasValue ? JsonConvert.DeserializeObject<Music>(musicData) : null;
        }

        //public async Task AddMusicAsync(Music music)
        //{
        //    music.Id = Guid.NewGuid().ToString();
        //    var musicData = JsonConvert.SerializeObject(music);
        //    await _db.StringSetAsync($"music:{music.Id}", musicData);
        //}

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

        //public async Task DeleteMusicAsync(string id)
        //{
        //    await _db.KeyDeleteAsync($"music:{id}");
        //}

        public async Task DeleteMusicAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Invalid music id", nameof(id));
            }

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