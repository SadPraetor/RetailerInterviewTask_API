using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class PaginationQuery {
        public int PageSize { get; set; } = 10;
        public int Page { get; set; } = 1;
    }
}
