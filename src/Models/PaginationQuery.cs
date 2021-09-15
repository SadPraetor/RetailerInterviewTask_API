using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class PaginationQuery {
        private int pageSize =10;

        public int PageSize {
            get => pageSize;
            set  {
                pageSize = value >100 ? 100 : value;
            } }
        public int Page { get; set; } = 1;

    }
}
