using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Faktologika.Ecommerce.Web
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext (DbContextOptions<CatalogDbContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; } = default!;
        public DbSet<ProductCategory> ProductCategory { get; set; } = default!;
    }
}
