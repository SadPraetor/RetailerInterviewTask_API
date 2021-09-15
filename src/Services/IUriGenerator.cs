using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services {
    public interface IUriGenerator {
        Dictionary<string, string> GeneratePaginationLinks<T>( PaginatedResponseModel<T> paginationResponseModel, string path );
    }
}
