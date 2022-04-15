using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Keyword
    {
        public long KeywordId { get; set; }
        public string? KeywordName { get; set; }
        public virtual List<Movie> Movies { get; set; } = new List<Movie>();

    }
}
