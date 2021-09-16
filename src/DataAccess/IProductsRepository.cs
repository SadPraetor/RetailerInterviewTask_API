using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace API.DataAccess {
    public interface IProductsRepository {

        
        public Task<IEnumerable<Product>> GetAllAsync( CancellationToken cancellationToken );
        Task<PaginatedResponseModel<Product>> GetAllPaginatedAsync( int page, int pageSize, CancellationToken cancellationToken );
        Task<Product> GetByIdAsync( int id, CancellationToken cancellationToken );
        Task<Product> UpdateDescription( int id, string newDescription, CancellationToken cancellationToken );
    }
}
