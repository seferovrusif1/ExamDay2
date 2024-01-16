using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class Day2DbContext : IdentityDbContext<AppUser>
    {
        public Day2DbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Item> Items { get; set; }
    }
}
