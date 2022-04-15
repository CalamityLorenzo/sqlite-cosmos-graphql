using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class MovieGenre
    {
        public long MovieId { get; set; }
        public long GenreId { get; set; }

        //public virtual Genre Genre { get; set; } = null!;
        //public virtual Movie Movie { get; set; } = null!;
    }
}
