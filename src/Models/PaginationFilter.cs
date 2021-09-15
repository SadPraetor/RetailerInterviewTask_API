using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models {
    public class PaginationFilter {

        public int PageSize { get; set; }
        public int Page { get; set; }
        public PaginationFilter() {
            PageSize = 10;
            Page = 1;
        }

        public PaginationFilter(PaginationQuery paginationQuery) {
            if ( paginationQuery == null ) {
                new PaginationFilter();
            }

            if ( paginationQuery.Page <1 ) {
                throw new FaultyPaginationQuery( "Page number must be greater than 0" );
            }

            if ( paginationQuery.PageSize <1 ) {
                throw new FaultyPaginationQuery( "Page size must be greater than 0" );
            }

            PageSize = paginationQuery.PageSize > 100 ? 100 : paginationQuery.PageSize; 
            Page = paginationQuery.Page;
        }



    }
}
