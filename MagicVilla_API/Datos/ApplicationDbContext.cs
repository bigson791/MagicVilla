using Microsoft.EntityFrameworkCore;
using MagicVilla_API.Modelos;

namespace MagicVilla_API.Datos
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
                
        }

        public DbSet<Villa> Villas { get; set; }
    }
}
