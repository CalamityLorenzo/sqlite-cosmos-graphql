using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Country
    {
        public Country()
        {
            Movies = new HashSet<Movie>();
        }

        public long CountryId { get; set; }
        public string CountryIsoCode { get; set; } = null!;
        public string CountryName { get; set; } = null!;

        public virtual ICollection<Movie> Movies { get; set; }
    }
}
