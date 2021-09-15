using API.Models;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services {
    public class UriGenerator:IUriGenerator {

        public Dictionary<string, string> GeneratePaginationLinks<T> (PaginatedResponseModel<T> paginationResponseModel, string path ) {

            //.net core 3.1, system.text.json does not support serializing of Ttype enum as dictionary key. Shame
            //need to use ToString, with .net 5 Dictionary<LinkType,string> is good to be used
            Dictionary<string, string> resourceLinks = null;

            if ( paginationResponseModel.TotalPages > paginationResponseModel.CurrentPage ) {
                resourceLinks ??= new Dictionary<string, string>();
                resourceLinks[LinkType.Next.ToString()] = QueryHelpers.AddQueryString(
                    path, new Dictionary<string, string>() { 
                        { "pageSize", paginationResponseModel.PageSize.ToString() },
                        { "page", (paginationResponseModel.CurrentPage + 1).ToString() }
                    } );

            }

            if ( paginationResponseModel.CurrentPage > 1 ) {
                resourceLinks ??= new Dictionary<string, string>();
                resourceLinks[LinkType.Prev.ToString()] = QueryHelpers.AddQueryString(
                    path, new Dictionary<string, string>() {
                        { "pageSize", paginationResponseModel.PageSize.ToString() },
                        { "page", (paginationResponseModel.CurrentPage - 1).ToString() }
                    } );
            }



            return resourceLinks;
        }
    }
}
