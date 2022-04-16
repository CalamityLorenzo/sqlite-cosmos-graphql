using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class MovieKeyword
    {
        public long MovieId { get; set; }
        public long KeywordId { get; set; }

        public virtual Keyword Keyword { get; set; } = null;
        //public virtual Movie Movie { get; set; } = null;
    }
}
    