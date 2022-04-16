using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class MovieGenreDb
    {
        public Guid id { get; set; }
        public string Name { get; set; }
    }
}
