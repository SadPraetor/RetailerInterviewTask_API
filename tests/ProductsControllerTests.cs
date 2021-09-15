using API.DataAccess;
using API.DevDataSeed;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using RetailerInterviewAPITask.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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

        [Fact]
        public async Task PaginateAsync_ShouldReturn10Products() {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof( this.PaginateAsync_ShouldReturn10Products ) );
            productsDbcontext.SeedAppDbContext(  new ProductFaker().GetFakeProducts( 20 ) );

            var result = await productsDbcontext.Products.PaginateAsync( 1, 10, new CancellationToken() );

            Assert.Equal( 10, result.Data.Count );

        }

        
        [Theory]
        [InlineData(1)]
        [InlineData( 25 )]
        [InlineData( 50 )]
        public async Task PaginateAsync_ShouldReturnNProducts(int pageSize) {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof( this.PaginateAsync_ShouldReturnNProducts ) );
            productsDbcontext.SeedAppDbContext( new ProductFaker().GetFakeProducts( 120 ) );

            var result = await productsDbcontext.Products.PaginateAsync( 1, pageSize, new CancellationToken() );

            Assert.Equal( pageSize, result.Data.Count );

        }


        [Fact]
        public async Task PaginateAsync_ShouldReturn2ndPage() {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof( this.PaginateAsync_ShouldReturn10Products ) );
            productsDbcontext.SeedAppDbContext( new ProductFaker().GetFakeProducts( 20 ) );

            var result = await productsDbcontext.Products.PaginateAsync( 2, 10, new CancellationToken() );

            Assert.Equal( 10, result.Data.Count );
            Assert.Equal( 11, result.Data.First().Id );
            Assert.Equal( 20, result.Data.Last().Id );
            Assert.Equal( 2, result.CurrentPage );

        }
        [Theory]
        [InlineData (49,1)]
        [InlineData( 25, 2 )]
        [InlineData( 1, 50 )]
        public async Task PaginateAsync_ShouldCalcualteTotalPages(int expected, int pageSize) {

            var productsDbcontext = SetupInMemoryDbContext.GetProductsDbContext( nameof( this.PaginateAsync_ShouldCalcualteTotalPages ) + pageSize.ToString()  );
            productsDbcontext.SeedAppDbContext( new ProductFaker().GetFakeProducts( 49 ) );

            var result = await productsDbcontext.Products.PaginateAsync( 1, pageSize, new CancellationToken() );

            Assert.Equal( expected, result.TotalPages );
        }

        [Fact]
        public void UriGenerator_ContainsNextDoesNotContainPrev () {

            var responseModel = new PaginatedResponseModel<Product>();
            var path = "api/products";

            responseModel.TotalPages = 10;
            responseModel.CurrentPage = 1;

            var generator = new UriGenerator( "https://domain.com" );

            var links = generator.GeneratePaginationLinks( responseModel, path );

            Assert.Contains<string>( LinkType.Next.ToString(), links.Select(x=>x.Key ));
            Assert.DoesNotContain<string>( LinkType.Prev.ToString(), links.Select( x => x.Key ) );

        }
        [Fact]
        public void UriGenerator_ContainsPrevDoesNotContainNext() {

            var responseModel = new PaginatedResponseModel<Product>();
            var path = "api/products";

            responseModel.TotalPages = 10;
            responseModel.CurrentPage = 10;

            var generator = new UriGenerator( "https://domain.com" );

            var links = generator.GeneratePaginationLinks( responseModel, path );

            Assert.Contains<string>( LinkType.Prev.ToString(), links.Select( x => x.Key ) );
            Assert.DoesNotContain<string>( LinkType.Next.ToString(), links.Select( x => x.Key ) );

        }

        [Fact]
        public void UriGenerator_ProperLinkGeneration() {

            var responseModel = new PaginatedResponseModel<Product>();
            var path = "api/products";

            responseModel.TotalPages = 10;
            responseModel.CurrentPage = 5;
            responseModel.PageSize = 10;

            var generator = new UriGenerator( "https://domain.com" );

            var links = generator.GeneratePaginationLinks( responseModel, path );

            Assert.Equal( "https://domain.com/api/products?pageSize=10&page=6", links["Next"] ,ignoreCase:true);
            Assert.Equal( "https://domain.com/api/products?pageSize=10&page=4", links["Prev"], ignoreCase: true );
        }

        [Fact]
        public void PaginationQuery_ShouldDefaultPage1PageSize10 () {

            var paginationFilter = new PaginationQuery();

            Assert.Equal( 1, paginationFilter.Page );
            Assert.Equal( 10, paginationFilter.PageSize );

        }

        [Fact]
        public void PaginationFilter_ShouldThrowException() {
            Assert.Throws<FaultyPaginationQueryException>( () => new PaginationFilter( -1, 10 ));
            Assert.Throws<FaultyPaginationQueryException>( () => new PaginationFilter( 1, -10 ) );
        }

        [Fact]
        public void PaginationFilter_LimitsTo100() {
            var sut = new PaginationFilter( 150, 5 );
            Assert.Equal( 100, sut.PageSize );
        }
    }
}
