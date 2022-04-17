using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public abstract record BaseMovieKeywordDb
    {
        public Guid id { get; set; }

        public string[] Keywords { get; set; } = new string[0];
        public virtual string entityType { get; set; }

    }

    public record MovieKeywordDb : BaseMovieKeywordDb
    {
        public MovieKeywordDb() 
        {
            this.entityType = "Keyword";
        }
        
        
    }

    public record MovieGenreKeywordDb : BaseMovieKeywordDb
    {
        public MovieGenreKeywordDb()
        {
            this.entityType = "Genre";
        }


    }
}
