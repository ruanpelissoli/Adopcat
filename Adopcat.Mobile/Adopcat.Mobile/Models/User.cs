﻿namespace Adopcat.Mobile.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FacebookId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public byte[] Picture { get; set; }
        public string PictureUrl { get; set; }
    }
}
