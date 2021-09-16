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
        public PaginationFilter(int pageSize, int page) {

            if ( page < 1 ) {
                throw new FaultyPaginationQueryException( "Page number must be greater than 0" );
            }

            if ( pageSize < 1 ) {
                throw new FaultyPaginationQueryException( "Page size must be greater than 0" );
            }

            PageSize =pageSize > 100 ? 100 : pageSize;
            Page = page;
        }

        public PaginationFilter(PaginationQuery paginationQuery) : this( paginationQuery.PageSize, paginationQuery.Page ) {
            if ( paginationQuery == null ) {
                new PaginationFilter();
            }            
        }



    }
}
