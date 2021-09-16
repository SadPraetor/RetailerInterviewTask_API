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
        private readonly IProductsRepository _productsRepository;
        private readonly IUriGenerator _uriGenerator;

        public ProductsController( 
            ILogger<ProductsController> logger,
            IProductsRepository productsRepository,
            IUriGenerator uriGenerator) 
        {
            _logger = logger;
            _productsRepository = productsRepository;
            _uriGenerator = uriGenerator;
        }


        
    }
}
