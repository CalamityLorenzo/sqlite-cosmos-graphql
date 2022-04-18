using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public abstract record BaseMovieKeywordDb
    {
        public Guid id { get; set; }

        public string[] Keywords { get; set; } = new string[0];
        internal virtual string entityType { get; }
    }

    public record MovieKeywordDb : BaseMovieKeywordDb
    {
         internal override string entityType => "Keyword";
    }

    public record MovieGenreKeywordDb : BaseMovieKeywordDb
    {
        internal override string entityType => "Genre";
    }
}
