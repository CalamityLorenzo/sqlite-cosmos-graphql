using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class MovieLanguage
    {
        public long MovieId { get; set; }
        public long LanguageId { get; set; }
        public long LanguageRoleId { get; set; }

        public virtual Language Language { get; set; } = null!;
        public virtual LanguageRole LanguageRole { get; set; } = null!;
        public virtual Movie Movie { get; set; } = null!;
    }
}
