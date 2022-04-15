using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Person
    {
        public long PersonId { get; set; }
        public string PersonName { get; set; }

        public virtual List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
