namespace MusicService.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int MusicId { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
