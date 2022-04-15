using System;
using System.Collections.Generic;

namespace sqlite.movies.Models
{
    public partial class Department
    {
        public long DepartmentId { get; set; }
        public string? DepartmentName { get; set; }
    }
}
