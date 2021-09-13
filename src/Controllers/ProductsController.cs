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
    public class ProductsController : ControllerBase {
        

        private readonly ILogger<ProductsController> _logger;
        private readonly ProductsDbContext _productsDbContext;

        public ProductsController( ILogger<ProductsController> logger,ProductsDbContext productsDbContext ) {
            _logger = logger;
            _productsDbContext = productsDbContext;
        }

        [Produces( "application/json" )]
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAll() {
            return await _productsDbContext.Products.AsNoTracking().ToListAsync();
        }

        [Produces( "application/json" )]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetById(int id) {
            var product =  await _productsDbContext.Products.FindAsync( id );

            if ( product == null )
                return NotFound();

            return Ok( product );
        }

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
