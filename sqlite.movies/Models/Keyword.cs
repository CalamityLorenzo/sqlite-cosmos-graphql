using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Keyword
    {
        public Keyword()
        {
            Movies = new HashSet<Movie>();
        }

        public long KeywordId { get; set; }
        public string KeywordName { get; set; } = null!;
        public virtual ICollection<Movie> Movies { get; set; }
    }
}
