using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Language
    {
        public long LanguageId { get; set; }
        public string LanguageCode { get; set; } = null!;
        public string LanguageName { get; set; } = null!;
        public virtual ICollection<Movie> Movies { get; set; }

    }
}
