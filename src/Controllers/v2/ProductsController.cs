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
        [ProducesResponseType( StatusCodes.Status400BadRequest, Type = typeof( ExceptionDto ) )]
        [HttpGet(Name =nameof(GetAllAsync20))]
        public async Task<ActionResult<PaginatedResponseModel<Product>>> GetAllAsync20( 
            [FromQuery] PaginationQuery paginationQuery, CancellationToken cancellationToken ) 
        {
            
            try {
                
                var paginationFilter = new PaginationFilter( paginationQuery );

                var paginatedModel =   await _productsDbContext
                    .Products
                    .AsNoTracking()
                    .PaginateAsync<Product>( paginationFilter.Page, paginationFilter.PageSize, cancellationToken ) ;

                var path = Url.RouteUrl( nameof( GetAllAsync20 ) );

                paginatedModel.Links = _uriGenerator.GeneratePaginationLinks<Product>(paginatedModel, path );

                return Ok(paginatedModel);

            }
            catch ( PageOutOfRangeException exception) {
                return NotFound( new ExceptionDto( exception ) );
            }
            catch ( Exception exception ) {              
                return BadRequest( new ExceptionDto(exception) );
            }


        }
       
    }
}
