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


        
    }
}
