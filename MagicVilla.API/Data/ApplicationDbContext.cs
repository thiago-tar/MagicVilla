using MagicVilla.API.Model;
using MagicVilla.API.Model.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MagicVilla.API.Data
{
    public class ApplicationDbContext : DbContext
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
            : base(options)
        {

        }
        public DbSet<Villa> Villas { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Villa>().HasData(
                new Villa() { Id = 1, Name = "Konohagakure", Ocurrency = 2, Sfto = 100 , CreateDate = DateTime.Now},
                new Villa() { Id = 2, Name = "Kirigakure", Ocurrency = 3, Sfto = 200, CreateDate = DateTime.Now }
                );
        }
    }
}
