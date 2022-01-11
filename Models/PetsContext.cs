using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace PetAdoptionApi.Models
{
    public class PetsContext : DbContext
    {
        public PetsContext(DbContextOptions<PetsContext> options) : base(options)
        {  
        }

        public DbSet<Pet> Pets { get; set; } = null!;
    }
}