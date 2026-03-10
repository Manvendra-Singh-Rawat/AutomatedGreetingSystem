using AutomatedGreetingSystem.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace AutomatedGreetingSystem.Infrastructure.Persistence.PostgreSQL
{
    public class AutoGreetDbContext : DbContext
    {
        public AutoGreetDbContext (DbContextOptions<AutoGreetDbContext> options) : base(options)
        {
        }

        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<Events> Events { get; set; }
    }
}
