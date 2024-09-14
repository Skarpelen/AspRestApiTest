using Microsoft.EntityFrameworkCore;

namespace AspRestApiTest.Data
{
    using AspRestApiTest.Data.Models;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Node> Nodes { get; set; }

        public DbSet<Tree> Trees { get; set; }

        public DbSet<ExceptionJournal> ExceptionJournals { get; set; }
    }
}
