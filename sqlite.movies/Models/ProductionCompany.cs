using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class ProductionCompany
    {
        public long CompanyId { get; set; }
        public string CompanyName { get; set; } = null!;
    }
}
