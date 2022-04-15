using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class ProductionCountry
    {
        public long? MovieId { get; set; }
        public long? CountryId { get; set; }

        public virtual Country? Country { get; set; }
        public virtual Movie? Movie { get; set; }
    }
}
