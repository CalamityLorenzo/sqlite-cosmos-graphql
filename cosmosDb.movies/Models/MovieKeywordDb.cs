using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class MovieKeywordDb
    {
        public Guid id { get; set; }
        public string KeywordName { get; set; }

    }
}
