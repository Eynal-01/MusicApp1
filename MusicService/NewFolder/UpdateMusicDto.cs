namespace MusicService.NewFolder
{
    public class UpdateMusicDto
    {
        public string Author { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public IFormFile MusicFile { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}
