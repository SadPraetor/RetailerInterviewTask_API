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
        [ProducesResponseType( StatusCodes.Status404NotFound )]
        [HttpGet]
        public async Task<ActionResult<PaginatedResponseModel<Product>>> GetAll20( 
            [FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken ) 
        {

            try {
                return Ok( await _productsDbContext
                    .Products
                    .AsNoTracking()
                    .PaginateAsync<Product>( paginationQuery.Page, paginationQuery.PageSize, cancellationToken ) );

            }
            catch ( PageOutOfRangeException exception ) {
              
                return NotFound( (new { Error=  exception.GetType().Name , Message= exception.Message }) );
            }


        }
       
    }
}
