using API.DataAccess;
using API.Models;
using API.Services;
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
        private readonly IUriGenerator _uriGenerator;

        public ProductsController( 
            ILogger<ProductsController> logger,
            ProductsDbContext productsDbContext,
            IUriGenerator uriGenerator) 
        {
            _logger = logger;
            _productsDbContext = productsDbContext;
            _uriGenerator = uriGenerator;
        }

        [MapToApiVersion( "1.0" )]
        [Produces( "application/json" )]
        [HttpGet]
        public async Task<IEnumerable<Product>> GetAllAsync() {
            return await _productsDbContext.Products.AsNoTracking().ToListAsync();
        }

        [MapToApiVersion( "1.0" )]
        [MapToApiVersion( "2.0" )]
        [Produces( "application/json" )]
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Product>> GetByIdAsync(int id) {
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
        public async Task<ActionResult<Product>> UpdateDescriptionAsync( int id , [FromBody] string newDescription ) {
            //TODO limit to 4000
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
