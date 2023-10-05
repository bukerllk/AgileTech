using AgileTech_API.Models;
using Microsoft.EntityFrameworkCore;

namespace AgileTech_API.Data
{
    public class ApplicatioDbContext :DbContext
    {
        public ApplicatioDbContext(DbContextOptions<ApplicatioDbContext> option) :base(option)
        {
            
        }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>().HasData(
                new Client() 
                { 
                    Id = 1,
                    Name = "Eduard Name",
                    Email = "eduard@gmail.com",
                    Created = DateTime.Now
                },
                new Client()
                {
                    Id = 2,
                    Name = "Alis Navarrete",
                    Email = "alis@gmail.com",
                    Created = DateTime.Now
                }
            );
        }
    }
}
