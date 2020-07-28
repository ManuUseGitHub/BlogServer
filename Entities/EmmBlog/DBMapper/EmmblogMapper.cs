using Entities.EmmBlog.DataModelObjects;
using Microsoft.EntityFrameworkCore;
using System;

namespace Entities.EmmBlog.DBMapper
{
    public class EmmblogMapper
    {
        private ModelBuilder ModelBuilder { get; set; }

        public EmmblogMapper(ModelBuilder modelBuilder)
        {
            ModelBuilder = modelBuilder;
        }

        public void MapSubscription()
        {
            ModelBuilder.Entity<Subscribe>()
                .HasKey(s => new { s.BlogId, s.AccountId });

            ModelBuilder.Entity<Subscribe>()
                .HasOne(s => s.Blog)
                .WithMany(b => b.Subscribe)
                .HasForeignKey(s => s.BlogId);

            ModelBuilder.Entity<Subscribe>()
                .HasOne(s => s.Account)
                .WithMany(a => a.Subscribe)
                .HasForeignKey(s => s.AccountId);
        }

        public void MapComments()
        {
            ModelBuilder.Entity<Comment>().HasKey(c => new { c.Id, c.RDepth});

            ModelBuilder.Entity<Comment>().HasMany(c => c.Answers)
                .WithOne(c => c.Answer)
                .HasForeignKey(c => new { c.Id, c.RDepth });

            ModelBuilder.Entity<Comment>()
                .HasOne(c => c.Answer)
                .WithMany(c => c.Answers)
                .HasForeignKey(c => new { c.AnswerId, c.AnswerRevision });
        }

        public void MapSharesAndArticles()
        {
            ModelBuilder.Entity<Article>()
                .HasOne(a => a.Blog)
                .WithMany(b => b.Articles)
                .HasForeignKey(a => a.BlogId);

            ModelBuilder.Entity<Comment>().HasOne(c => c.Article)
                .WithMany(a => a.Comments)
                .HasForeignKey(c => new { c.Slug, c.VDepth, c.BlogId});

            ModelBuilder.Entity<Article>()
                .HasKey(a => new { a.Slug, a.VDepth, a.BlogId });

            ModelBuilder.Entity<Share>()
                .HasKey(s => new { s.Slug, s.VDepth, s.SharingBlogId, s.BlogId });

            ModelBuilder.Entity<Share>()
                .HasOne(s => s.Blog)
                .WithMany(b => b.Shares)
                .HasForeignKey(s => s.SharingBlogId);

            ModelBuilder.Entity<Share>()
                .HasOne(s => s.Article)
                .WithMany(a => a.Shares)
                .HasForeignKey(s => new { s.Slug, s.VDepth, s.BlogId });
        }

        internal void MapMessages()
        {
            ModelBuilder.Entity<Message>()
                .HasKey(m => new { m.MessageId, m.FromId, m.ToId});

            ModelBuilder.Entity<Message>()
                .HasOne(m => m.Answer)
                .WithOne()
                .HasForeignKey<Message>(m => 
                    new { m.AnswerId, m.AnswerFromId, m.AnswerToId}
                );

            ModelBuilder.Entity<Message>()
                .HasOne(m => m.From)
                .WithMany()
                .HasForeignKey(m => m.FromId);

            ModelBuilder.Entity<Message>()
                .HasOne(m => m.To)
                .WithMany()
                .HasForeignKey(m => m.ToId);
        }

        internal void MapFollows()
        {
            ModelBuilder.Entity<Follow>()
                .HasKey(f => new { f.FollowingId, f.FollowedId });

            ModelBuilder.Entity<Follow>()
                .HasOne(f => f.Following)
                .WithMany(a => a.Followings)
                .HasForeignKey(f => f.FollowingId);

            ModelBuilder.Entity<Follow>()
                .HasOne(f => f.Followed)
                .WithMany(a => a.Followers)
                .HasForeignKey(f => f.FollowedId);
        }

        public void MapConnexions()
        {
            ModelBuilder.Entity<Connexion>()
                .HasKey(c => new { c.Id , c.AccountId});
        }
        internal void MapReact()
        {
            ModelBuilder.Entity<React>()
                .HasKey(r => new { r.AccountId, r.TypeId, r.ItemId });

            ModelBuilder.Entity<React>()
                .HasOne(r => r.Article)
                .WithMany(a => a.Reactions)
                .HasForeignKey(r => new { r.Slug, r.VDepth, r.BlogId});

            ModelBuilder.Entity<React>()
                .HasOne(r => r.Comment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(r => new { r.CommentId, r.RDepth});

        }
    }
}
