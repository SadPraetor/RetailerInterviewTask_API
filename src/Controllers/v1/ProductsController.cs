using API.DataAccess;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace RetailerInterviewAPITask.Controllers {
   
    public partial class ProductsController : ControllerBase {
        

        /// <summary>
        /// Returns list of products
        /// </summary>                
        /// <response code="200">Products found and returned</response>                   
        [HttpGet]
        [MapToApiVersion( "1.0" )]
        [Produces( "application/json" )]
        [ProducesResponseType( StatusCodes.Status200OK, Type = typeof( IEnumerable<Product> ) )]
        public async Task<IEnumerable<Product>> GetAllAsync( CancellationToken cancellationToken ) {
            return await _productsDbContext.Products.AsNoTracking().ToListAsync( cancellationToken );
        }


        /// <summary>
        /// Returns specific requested product, by Id
        /// </summary>                
        /// <param name="id">Id of the product</param> 
        /// <response code="200">Product found and returned</response>  
        /// <response code="404">Requested product was not found</response> 
        [HttpGet("{id:int}")]
        [MapToApiVersion( "1.0" )]
        [MapToApiVersion( "2.0" )]
        [Produces( "application/json" )]
        [ProducesResponseType( StatusCodes.Status200OK, Type = typeof( Product ) )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ExceptionDto ) )]
        public async Task<ActionResult<Product>> GetByIdAsync(int id, CancellationToken cancellationToken ) {

            var product =  await _productsDbContext.Products.FindAsync(new object[] { id }, cancellationToken );

            if ( product == null )
                return NotFound(new ExceptionDto ("NotFound","Requested product was not found"));

            return Ok( product );
        }


        /// <summary>
        /// Updates description on specific product, identified by Id
        /// </summary>               
        /// <param name="id">Id of the product</param> 
        /// <param name="newDescription">Text/Plain new description. Limit 4000 characters</param> 
        /// <response code="200">Product found and description updated</response>  
        /// <response code="404">New description is too long</response> 
        /// <response code="404">Product not found</response> 
        [HttpPatch( "{id:int}/description" )]
        [MapToApiVersion( "1.0" )]
        [MapToApiVersion( "2.0" )]
        [Consumes("text/plain")]
        [Produces( "application/json" )]
        [ProducesResponseType( StatusCodes.Status200OK, Type = typeof( Product ) )]
        [ProducesResponseType( StatusCodes.Status400BadRequest, Type = typeof( ExceptionDto ) )]
        [ProducesResponseType( StatusCodes.Status404NotFound, Type = typeof( ExceptionDto ) )]
        public async Task<ActionResult<Product>> UpdateDescriptionAsync( int id , [FromBody] string newDescription, CancellationToken cancellationToken ) {

            var lengthLimit =  typeof( Product )
                .GetProperty( nameof( Product.Description ) )
                .GetCustomAttributes( typeof( StringLengthAttribute ), false )
                .OfType<StringLengthAttribute>()
                .FirstOrDefault()?
                .MaximumLength ?? int.MaxValue;

            if ( newDescription.Length>lengthLimit ) {
                return BadRequest( new ExceptionDto( "DescriptionTooLong", $"Description is limited to {lengthLimit} characters" ) );
            }

            var product = await _productsDbContext.Products.FindAsync( new object[] { id }, cancellationToken );

            if ( product == null )
                return NotFound( new ExceptionDto( "NotFound", "Requested product was not found" ) );

            product.Description = newDescription;

            

            //TODO check for concurrency error?
            await _productsDbContext.SaveChangesAsync();

            return Ok( product );
        }
    }
}
