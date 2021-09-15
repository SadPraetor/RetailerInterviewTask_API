using API.DataAccess;
using API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace RetailerInterviewAPITask.Controllers {
   
    public partial class ProductsController : ControllerBase {

        [MapToApiVersion( "2.0" )]
        [Produces( "application/json" )]
        [ProducesResponseType( StatusCodes.Status200OK, Type = typeof( Product ) )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ExceptionDto ) )]
        [HttpGet]
        public async Task<ActionResult<PaginatedResponseModel<Product>>> GetAll20( 
            [FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken ) 
        {
            //TODO handle bad pagination query

            try {
                var paginationFilter = new PaginationFilter( paginationQuery );

                return Ok( await _productsDbContext
                    .Products
                    .AsNoTracking()
                    .PaginateAsync<Product>( paginationFilter.Page, paginationFilter.PageSize, cancellationToken ) );

            }
            catch ( Exception exception ) {
              
                return NotFound( new ExceptionDto(exception) );
            }


        }
       
    }
}
