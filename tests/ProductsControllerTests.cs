using API.DataAccess;
using API.DevDataSeed;
using API.Models;
using Microsoft.AspNetCore.Mvc;
using RetailerInterviewAPITask.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace RetailApiTestProject {
    public class ProductsControllerTests {

        
        private static List<Product> _productsSeed;
        public ProductsControllerTests() {

            _productsSeed = new ProductFaker().GetFakeProducts( 10 );
            
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllProducts() {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof(this.GetAll_ShouldReturnAllProducts) );
            productsDbcontext.SeedAppDbContext( _productsSeed );

            var controller = new ProductsController( null, productsDbcontext, null );

            var result = await controller.GetAllAsync();

            Assert.Equal( _productsSeed.Count(), result.Count() );

        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult() {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof( this.GetById_ShouldReturnOkResult ) );
            productsDbcontext.SeedAppDbContext( _productsSeed );

            var controller = new ProductsController( null, productsDbcontext, null );

            var result = await controller.GetByIdAsync(_productsSeed.Count);

            Assert.IsType<OkObjectResult>( result.Result );

        }

        [Fact]
        public async Task GetById_ShouldReturnNotFoundObjectResult() {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof( this.GetById_ShouldReturnNotFoundObjectResult) );
            productsDbcontext.SeedAppDbContext( _productsSeed );

            var controller = new ProductsController( null, productsDbcontext, null );

            var result = await controller.GetByIdAsync( _productsSeed.Count+5 );

            Assert.IsType<NotFoundObjectResult>( result.Result );

        }

        [Fact]
        public async Task GetById_ShouldReturnCorrespondingProduct() {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof( this.GetById_ShouldReturnCorrespondingProduct ) );
            productsDbcontext.SeedAppDbContext( _productsSeed );

            var controller = new ProductsController( null, productsDbcontext,null );

            var result = await controller.GetByIdAsync( _productsSeed.Count );

            Assert.IsType<OkObjectResult>( result.Result );
            Assert.NotNull( result.Result );

            Product product=null;
            if ( result.Result != null && result.Result is OkObjectResult ) {
                product = (Product)( (ObjectResult)result.Result ).Value;
                Assert.Equal( _productsSeed.Count, product.Id );
            }
        }

        [Fact]
        public async Task UpdateDescription_ShouldReturnNotFoundObjectResult() {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof( this.UpdateDescription_ShouldReturnNotFoundObjectResult ) );
            productsDbcontext.SeedAppDbContext( _productsSeed );

            var controller = new ProductsController( null, productsDbcontext, null );

            var result = await controller.UpdateDescriptionAsync( _productsSeed.Count +5, "test" );

            Assert.IsType<NotFoundObjectResult>( result.Result );

        }

        [Fact]
        public async Task UpdateDescription_ShouldReturnOk() {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof( this.UpdateDescription_ShouldReturnOk ) );
            productsDbcontext.SeedAppDbContext( _productsSeed );

            var controller = new ProductsController( null, productsDbcontext , null);

            var result = await controller.UpdateDescriptionAsync( _productsSeed.Count , "test" );

            Assert.IsType<OkObjectResult>( result.Result );

        }

        [Fact]
        public async Task UpdateDescription_ShouldUpdateDescriptionField() {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof( this.UpdateDescription_ShouldUpdateDescriptionField ) );
            productsDbcontext.SeedAppDbContext( _productsSeed );

            var guid = Guid.NewGuid();

            var controller = new ProductsController( null, productsDbcontext, null );

            var result = await controller.UpdateDescriptionAsync( _productsSeed.Count, guid.ToString() );


            Assert.IsType<OkObjectResult>( result.Result );
            Assert.NotNull( result.Result );

            if ( result.Result != null && result.Result is OkObjectResult ) {
                var product = await productsDbcontext.Products.FindAsync( _productsSeed.Count );

                var returnedProduct = (Product)( (ObjectResult)result.Result ).Value;
                Assert.Equal( product.Id, returnedProduct.Id );
                Assert.Equal( product.Description, returnedProduct.Description );
                Assert.Equal( guid.ToString(), returnedProduct.Description );
            }
        }
    }
}
