﻿namespace MusicService.NewFolder
{
    public class UserProfileUpdateDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public IFormFile ProfilePhoto { get; set; }
    }


}
