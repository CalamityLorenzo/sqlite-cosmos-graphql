using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Gender
    {
        public Gender()
        {
            MovieCasts = new HashSet<MovieCast>();
        }

        public long GenderId { get; set; }
        public string? Gender1 { get; set; }

        public virtual ICollection<MovieCast> MovieCasts { get; set; }
    }
}
