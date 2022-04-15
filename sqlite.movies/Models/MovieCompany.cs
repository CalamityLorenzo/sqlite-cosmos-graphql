using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class MovieCompany
    {
        public long? MovieId { get; set; }
        public long? CompanyId { get; set; }

        public virtual ProductionCompany? Company { get; set; }
        public virtual Movie? Movie { get; set; }
    }
}
