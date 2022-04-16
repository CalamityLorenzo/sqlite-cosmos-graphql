using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class MovieCast
    {
        public long MovieId { get; set; }
        public long PersonId { get; set; }
        public string CharacterName { get; set; } = null!;
        public long GenderId { get; set; }
        public long CastOrder { get; set; }

        public virtual Gender Gender { get; set; } = null!;
        public virtual Movie Movie { get; set; } = null!;
        public virtual Person Person { get; set; } = null!;
    }
}
