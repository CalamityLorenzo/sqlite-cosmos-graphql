using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class MovieDb
    {
        public Guid id { get; set; }
        public string Title { get; set; }
        public long Budget { get; set; }
        public string Homepage { get; set; }
        public string Overview { get; set; }
        public double Popularity { get; set; }
        public DateTime ReleaseDate { get; set; }
        public long  Revenue { get; set; }
        public long Runtime { get; set; }
        public string MovieStatus { get; set; }
        public string Tagline { get; set; }
        public double VoteAverage { get; set; }
        public long VoteCount { get; set; }
        public List<String> Keywords { get; set; }
        public List<String> Genres { get; set; }

    }
}
