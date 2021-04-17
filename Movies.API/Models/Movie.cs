using Movies.API.Models.Base;
using System;

namespace Movies.API.Models
{
    public class Movie : Entity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public string Owner { get; set; }
        public string Rating { get; set; }
        public string ImageUrl { get; set; }
    }
}
