using API.DataAccess;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RetailerInterviewAPITask.Controllers {
    [ApiController]
    [Route( "api/[controller]" )]
    [ApiVersion( "1.0" )]
    [ApiVersion( "2.0" )]
    public partial class ProductsController : ControllerBase {
        

        private readonly ILogger<ProductsController> _logger;
        private readonly ProductsDbContext _productsDbContext;

        public ProductsController( ILogger<ProductsController> logger,ProductsDbContext productsDbContext ) {
            _logger = logger;
            _productsDbContext = productsDbContext;
        }

        [MapToApiVersion( "1.0" )]
        [Produces( "application/json" )]
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAll() {
            return await _productsDbContext.Products.AsNoTracking().ToListAsync();
        }

        [MapToApiVersion( "1.0" )]
        [MapToApiVersion( "2.0" )]
        [Produces( "application/json" )]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id) {
            var product =  await _productsDbContext.Products.FindAsync( id );

            if ( product == null )
                return NotFound();

            return Ok( product );
        }


        [MapToApiVersion( "1.0" )]
        [MapToApiVersion( "2.0" )]
        [Consumes("text/plain")]
        [Produces( "application/json" )]        
        [HttpPatch( "{id:int}/update-description" )]
        public async Task<ActionResult<Product>> UpdateDescription( int id , [FromBody] string newDescription ) {

            var product = await _productsDbContext.Products.FindAsync( id );

            if ( product == null )
                return NotFound();

            product.Description = newDescription;

            //TODO check for concurrency error?
            await _productsDbContext.SaveChangesAsync();

            return Ok( product );
        }
    }
}
