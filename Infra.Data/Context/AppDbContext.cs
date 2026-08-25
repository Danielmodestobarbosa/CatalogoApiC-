
using CatalogoApi.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CatalogoApi.Infra.Data.Context;

public class AppDbContext : IdentityDbContext<AplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }

        protected override void OnModelCreating (ModelBuilder builder)
        {
        base.OnModelCreating(builder);
        }


    }

