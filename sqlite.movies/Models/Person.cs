using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Person
    {
        public Person()
        {
            MovieCasts = new HashSet<MovieCast>();
        }

        public long PersonId { get; set; }
        public string? PersonName { get; set; }

        public virtual ICollection<MovieCast> MovieCasts { get; set; }
    }
}
