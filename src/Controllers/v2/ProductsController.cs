using API.DataAccess;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RetailerInterviewAPITask.Controllers {
   
    public partial class ProductsController : ControllerBase {

        [MapToApiVersion( "2.0" )]
        [Produces( "application/json" )]
        [HttpGet]
        public async Task<PaginatedResponseModel<Product>> GetAll20( CancellationToken cancellationToken ) {

            return await _productsDbContext.Products.AsNoTracking().PaginateAsync<Product>( 1, 10, cancellationToken );
        }
       
    }
}
