using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.DataAccess {
    public class DbProductsRepository : IProductsRepository {
        private readonly ProductsDbContext _productsDbContext;

        private static readonly int _descriptionMaxLength = typeof( Product )
                .GetProperty( nameof( Product.Description ) )
                .GetCustomAttributes( typeof( StringLengthAttribute ), false )
                .OfType<StringLengthAttribute>()
                .FirstOrDefault()?
                .MaximumLength ?? int.MaxValue;

        public DbProductsRepository(ProductsDbContext dbContext) {
            _productsDbContext = dbContext;
        }
        public async Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken) {
            return await _productsDbContext.Products.AsNoTracking().ToListAsync( cancellationToken );
        }

        public async Task<Product> GetByIdAsync (int id, CancellationToken cancellationToken ) {
            return await _productsDbContext.Products.FindAsync( new object[] { id }, cancellationToken );
        }

        public async Task<Product> UpdateDescription( int id,string newDescription, CancellationToken cancellationToken ) {

            var lengthLimit = _descriptionMaxLength;

            if ( newDescription.Length > lengthLimit ) {
                throw new DescriptionTooLongException();
            }

            var product = await GetByIdAsync(  id , cancellationToken );

            if ( product == null )
                return null;

            product.Description = newDescription;

            //TODO check for concurrency error?
            await _productsDbContext.SaveChangesAsync();

            return product;
        }

        public async Task<PaginatedResponseModel<Product>> GetAllPaginatedAsync(int page, int pageSize, CancellationToken cancellationToken) {

            return await _productsDbContext
                    .Products
                    .AsNoTracking()
                    .OrderBy( x => x.Id )
                    .PaginateAsync<Product>( page, pageSize, cancellationToken );

        }
    }
}
