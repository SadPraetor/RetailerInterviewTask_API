using API.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DataAccess {
    public class ProductsDbContext:DbContext {

        public ProductsDbContext( DbContextOptions<ProductsDbContext> options )
            : base( options ) {
        }

        public DbSet<Product> Products { get; set; }

    }
}
