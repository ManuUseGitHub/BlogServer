using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DBMapper;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class RepositoryContext : DbContext
    {
        public RepositoryContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Follow> Follow { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Credential> Credentials { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Subscribe> Subscribe { get; set; }
        public DbSet<Article> Article { get; set; }
        public DbSet<React> Reactions { get; set; }
        public DbSet<ReactionType> ReactionTypes { get; set; }
        public DbSet<Connexion> Connexions { get; set; }
        public DbSet<Share> Shares { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging(true);
            optionsBuilder.EnableServiceProviderCaching(false);
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            EmmblogMapper mapper = new EmmblogMapper(modelBuilder);
            mapper.MapSubscription();
            mapper.MapReact();
            mapper.MapConnexions();
            mapper.MapSharesAndArticles();
            mapper.MapComments();
            mapper.MapMessages();
            mapper.MapFollows();
        }
    }
}
