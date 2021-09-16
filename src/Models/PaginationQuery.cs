using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class PaginationQuery {

        /// <summary> 
        /// Number of items returned in one request 
        /// </summary>       
        public int PageSize { get; set; } = 10;
        /// <summary> 
        /// Requested page 
        /// </summary>  
        public int Page { get; set; } = 1;

        public PaginationQuery(int pageSize , int page) {
            PageSize = pageSize;
            Page = page;
        }

        public PaginationQuery() {

        }
        
    }
}
