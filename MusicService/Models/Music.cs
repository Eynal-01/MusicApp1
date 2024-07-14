namespace MusicService.Models
{
    //public class Music
    //{
    //    public string? Id { get; set; }
    //    public string? Author { get; set; }
    //    public string? Title { get; set; }
    //    public string? Description { get; set; }
            //public string? FilePath { get; set; } // Path to the music file
    //    public int LikeCount { get; set; }
    //}


        public class Music
        {
            public string Id { get; set; }
            public string Author { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string UserId { get; set; } // Add this property
            public int LikeCount { get; set; }
            public string? FilePath { get; set; } // Path to the music file
        }

}
