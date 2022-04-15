using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Movie
    {
        public long MovieId { get; set; }
        public string? Title { get; set; }
        public long? Budget { get; set; }
        public string? Homepage { get; set; }
        public string? Overview { get; set; }
        public double? Popularity { get; set; }
        public byte[]? ReleaseDate { get; set; }
        public long? Revenue { get; set; }
        public long? Runtime { get; set; }
        public string? MovieStatus { get; set; }
        public string? Tagline { get; set; }
        public double? VoteAverage { get; set; }
        public long? VoteCount { get; set; }

        public virtual List<Keyword> Keywords { get; set; } = new List<Keyword>();
        public virtual List<Genre> Genres { get; set; } = new List<Genre>();
        public virtual List<Person> Cast { get; set; } = new List<Person>();

    }
}
