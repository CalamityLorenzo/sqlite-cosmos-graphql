using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Genre
    {
        public Genre()
        {
            Movies = new HashSet<Movie>();
        }

        public long GenreId { get; set; }
        public string GenreName { get; set; } = null!;

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
