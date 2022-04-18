using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public abstract record BaseMovieKeywordDb
    {
        public Guid id { get; set; }

        public string[] Keywords { get; set; } = new string[0];
        public virtual string entityType { get; }
    }

    public record MovieKeywordDb(
        Guid id,
        string[] Keywords
        )
    {
        public string entityType => "Keyword";
    }

    public record MovieGenreDb(
        Guid id,
        string[] Genres
        )
    {
        public string entityType => "Genre";
    }
}
