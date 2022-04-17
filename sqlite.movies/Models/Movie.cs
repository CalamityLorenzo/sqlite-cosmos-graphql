using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Movie
    {
        public Movie()
        {
            MovieCasts = new HashSet<MovieCast>();
            Countries = new HashSet<Country>();
            Genres = new HashSet<Genre>();
            Keywords = new HashSet<Keyword>();
        }

        public long MovieId { get; set; }
        public string Title { get; set; } = null!;
        public long Budget { get; set; }
        public string Homepage { get; set; } = null!;
        public string Overview { get; set; }
        public double Popularity { get; set; }
        public byte[]? ReleaseDate { get; set; }
        public long? Revenue { get; set; }
        public long? Runtime { get; set; }
        public string? MovieStatus { get; set; }
        public string  Tagline { get; set; }
        public double VoteAverage { get; set; }
        public long VoteCount { get; set; }

        public virtual ICollection<MovieCast> MovieCasts { get; set; }

        public virtual ICollection<Country> Countries { get; set; }
        public virtual ICollection<Genre> Genres { get; set; }
        public virtual ICollection<Keyword> Keywords { get; set; }
        public virtual ICollection<Language> Languages { get; set; }

    }
}
