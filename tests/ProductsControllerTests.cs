using API.DataAccess;
using API.DevDataSeed;
using API.Models;
using API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Moq;
using RetailerInterviewAPITask.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        private static ActionContext GetActionContextForPage( string page ) {
            return new ActionContext() {
                ActionDescriptor = new ActionDescriptor() {
                    RouteValues = new Dictionary<string, string>
                    {
                { "page", page },
            }
                },
                RouteData = new RouteData() {
                    Values =
                    {
                [ "page" ] = page
            }
                }
            };
        }

        [Fact]
        public async Task GetAll_ShouldReturnAllProducts() {

            var productsRepositoryMock = new Mock<IProductsRepository>();
            productsRepositoryMock.Setup( x => x.GetAllAsync( new CancellationToken() ) ).Returns(Task.FromResult( _productsSeed.AsEnumerable() ));

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, null );

            var result = await controller_sut.GetAllAsync(new CancellationToken());

            Assert.Equal( _productsSeed.Count(), result.Count() );

        }

        [Fact]
        public async Task GetById_ShouldReturnOkResult() {

            var productsRepositoryMock = new Mock<IProductsRepository>();
            productsRepositoryMock.Setup( x => x.GetByIdAsync(It.IsAny<int>(), new CancellationToken() ) ).Returns( Task.FromResult( _productsSeed.First() ) );

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, null );

            var result = await controller_sut.GetByIdAsync( _productsSeed.Count, new CancellationToken() );

            Assert.IsType<OkObjectResult>( result.Result );

        }

        [Fact]
        public async Task GetById_ShouldReturnNotFoundObjectResult() {

            var productsRepositoryMock = new Mock<IProductsRepository>();
            productsRepositoryMock.Setup( x => x.GetByIdAsync( It.IsAny<int>(), new CancellationToken() ) ).Returns(Task.FromResult( (Product)null ));

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, null );

            var result = await controller_sut.GetByIdAsync( 1, new CancellationToken() );

            Assert.IsType<NotFoundObjectResult>( result.Result );

        }

        [Theory]
        [InlineData(1)]
        [InlineData( 5 )]
        [InlineData( 7 )]
        public async Task GetById_ShouldReturnCorrespondingProduct(int id) {

            var productsRepositoryMock = new Mock<IProductsRepository>();
            productsRepositoryMock.Setup( x => x.GetByIdAsync(id, new CancellationToken() ) ).Returns( Task.FromResult( _productsSeed[id-1] ) );

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, null );

            var result = await controller_sut.GetByIdAsync( id, new CancellationToken() );

            Assert.IsType<OkObjectResult>( result.Result );
            Assert.NotNull( result.Result );

            Product product = null;
            product = (Product)( (ObjectResult)result.Result ).Value;
            Assert.Equal( _productsSeed[id - 1].Id, product.Id );
            
        }

        [Fact]
        public async Task UpdateDescription_ShouldReturnNotFoundObjectResult() {

            var productsRepositoryMock = new Mock<IProductsRepository>();
            productsRepositoryMock.Setup( x => x.GetByIdAsync( It.IsAny<int>(), new CancellationToken() ) )
                .Returns( Task.FromResult( (Product)null ) );

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, null );

            var result = await controller_sut.UpdateDescriptionAsync(1, "test", new CancellationToken() );

            Assert.IsType<NotFoundObjectResult>( result.Result );

        }

        [Fact]
        public async Task UpdateDescription_ShouldReturnOk() {

            var productsRepositoryMock = new Mock<IProductsRepository>();            
            productsRepositoryMock.Setup( x => x.UpdateDescription( It.IsAny<int>(), It.IsAny<string>(), new CancellationToken() ) )
                .Returns( Task.FromResult( ( _productsSeed.Last() ) ) );

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, null );
            

            var result = await controller_sut.UpdateDescriptionAsync( _productsSeed.Count, "test", new CancellationToken() );

            Assert.IsType<OkObjectResult>( result.Result );

        }

        [Fact]
        public async Task UpdateDescription_ShouldReturnBadRequestObjectResult() {

            var lengthLimit = typeof( Product )
               .GetProperty( nameof( Product.Description ) )
               .GetCustomAttributes( typeof( StringLengthAttribute ), false )
               .OfType<StringLengthAttribute>()
               .FirstOrDefault()?
               .MaximumLength;

            Assert.NotNull( lengthLimit );

            var productsRepositoryMock = new Mock<IProductsRepository>();            
            productsRepositoryMock.Setup( x => x.UpdateDescription( It.IsAny<int>(), It.IsAny<string>(), new CancellationToken() ) )
                .Throws( new DescriptionTooLongException() );

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, null );
            

            var result = await controller_sut.UpdateDescriptionAsync( _productsSeed.Count, "test", new CancellationToken() );

            Assert.IsType<BadRequestObjectResult>( result.Result );

        }

        [Fact]
        public async Task UpdateDescription_ShouldReturnUpdatedProduct() {

            //Arrange
            var guid = Guid.NewGuid();

            var productsRepositoryMock = new Mock<IProductsRepository>();
            productsRepositoryMock.Setup( x => x.UpdateDescription( It.IsAny<int>(), It.IsAny<string>(), new CancellationToken() ) )
                .Returns( Task.FromResult( new Product() { Description = guid.ToString() } ));

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, null );

            //act
            var result = await controller_sut.UpdateDescriptionAsync( _productsSeed.Count, "test", new CancellationToken() );

            //assert
            Assert.IsType<OkObjectResult>( result.Result );

            Product product = null;
            product = (Product)( (ObjectResult)result.Result ).Value;

            Assert.Equal( guid.ToString(), product.Description );

        }




        [Fact]
        public async Task GetAll20_ShouldReturnOkObject() {

            //Arrange
            var productsRepositoryMock = new Mock<IProductsRepository>();
            productsRepositoryMock.Setup( x => x.GetAllPaginatedAsync( It.IsAny<int>(), It.IsAny<int>(), new CancellationToken() ) )
                .Returns( Task.FromResult( new PaginatedResponseModel<Product>() ) );
            
            var context = GetActionContextForPage( "/api/products" );

            var uriGeneratorMock = new Mock<IUriGenerator>();
            uriGeneratorMock.Setup( x =>
             x.GeneratePaginationLinks( It.IsAny<PaginatedResponseModel<Product>>(), It.IsAny<string>() ) )
            .Returns( new Dictionary<string, string> { { "Next", "xyz" } } );

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.SetupGet( x => x.ActionContext )
                .Returns( context );

            urlHelperMock.Setup( x => x.RouteUrl( It.IsAny<UrlRouteContext>() ) )
                .Returns( "api/products" );

            var ctx = new DefaultHttpContext();

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, uriGeneratorMock.Object );
            controller_sut.ControllerContext = new ControllerContext() { HttpContext = ctx };
            controller_sut.Url = urlHelperMock.Object;

            //Act
            var response = await controller_sut.GetAllAsync20( new PaginationQuery(), new CancellationToken() );

            //Assert
            Assert.IsType<OkObjectResult>( response.Result );

        }

        [Fact]
        public async Task GetAll20_ShouldReturnNotFoundObjectResult() {

            //Arrange
            var productsRepositoryMock = new Mock<IProductsRepository>();
            productsRepositoryMock.Setup( x => x.GetAllPaginatedAsync( It.IsAny<int>(), It.IsAny<int>(), new CancellationToken() ) )
                .Throws( new PageOutOfRangeException() );


            var context = GetActionContextForPage( "/api/products" );

            var uriGeneratorMock = new Mock<IUriGenerator>();
            uriGeneratorMock.Setup( x =>
             x.GeneratePaginationLinks( It.IsAny<PaginatedResponseModel<Product>>(), It.IsAny<string>() ) )
            .Returns( new Dictionary<string, string> { { "Next", "xyz" } } );

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.SetupGet( x => x.ActionContext )
                .Returns( context );

            urlHelperMock.Setup( x => x.RouteUrl( It.IsAny<UrlRouteContext>() ) )
                .Returns( "api/products" );

            var ctx = new DefaultHttpContext();

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, uriGeneratorMock.Object );
            controller_sut.ControllerContext = new ControllerContext() { HttpContext = ctx };
            controller_sut.Url = urlHelperMock.Object;


            //Act
            var response = await controller_sut.GetAllAsync20( new PaginationQuery( 10, 20 ), new CancellationToken() );

            //Assert
            Assert.IsType<NotFoundObjectResult>( response.Result );

        }

        [Fact]
        public async Task GetAll20_ShouldReturnBadRequestObjectResult() {

            //Arrange
            var productsRepositoryMock = new Mock<IProductsRepository>();
            productsRepositoryMock.Setup( x => x.GetAllPaginatedAsync( It.IsAny<int>(), It.IsAny<int>(), new CancellationToken() ) )
                .Returns( Task.FromResult( new PaginatedResponseModel<Product>() ) );

            var context = GetActionContextForPage( "/api/products" );

            var uriGeneratorMock = new Mock<IUriGenerator>();
            uriGeneratorMock.Setup( x =>
             x.GeneratePaginationLinks( It.IsAny<PaginatedResponseModel<Product>>(), It.IsAny<string>() ) )
            .Returns( new Dictionary<string, string> { { "Next", "xyz" } } );

            var urlHelperMock = new Mock<IUrlHelper>();
            urlHelperMock.SetupGet( x => x.ActionContext )
                .Returns( context );

            urlHelperMock.Setup( x => x.RouteUrl( It.IsAny<UrlRouteContext>() ) )
                .Returns( "api/products" );

            var ctx = new DefaultHttpContext();

            var controller_sut = new ProductsController( null, productsRepositoryMock.Object, uriGeneratorMock.Object );
            controller_sut.ControllerContext = new ControllerContext() { HttpContext = ctx };
            controller_sut.Url = urlHelperMock.Object;


            //Act
            var response = await controller_sut.GetAllAsync20( new PaginationQuery( 10, -20 ), new CancellationToken() );

            //Assert
            Assert.IsType<BadRequestObjectResult>( response.Result );

        }
    }
}
