using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Genre
    {
        public long GenreId { get; set; }
        public string GenreName { get; set; }
        public virtual List<Movie> Movies { get; set; } = new List<Movie>();
    }
}
