using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Utilities;
using System;
using System.Linq;

namespace Repository
{
    public class ShareRepository : RepositoryBase<Share> , IShareRepository
    {
        public ShareRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        public Share getShare(Share shared)
        {
            return RepositoryContext.Shares
                .Where(s => s.Slug.Equals(shared.Slug))
                .Where(s => s.BlogId.Equals(shared.BlogId))
                .Where(s => s.VDepth.Equals(shared.VDepth))
                .Where(s => s.SharingBlogId.Equals(shared.SharingBlogId))
                .FirstOrDefault();
        }

        public Share getShare(Article article)
        {
            return RepositoryContext.Shares
                .Where(s => s.Slug.Equals(article.Slug))
                .Where(s => s.BlogId.Equals(article.BlogId))
                .Where(s => s.VDepth.Equals(article.VDepth))
                .FirstOrDefault();
        }

        public Share ShareArticle(Article article,Blog blog2)
        {
            Article merged = new Article()
            {
                Blog = blog2,
                BlogId = blog2.Id,
                VDepth = 0,
            };

            Reflector.Merge(merged, article, Reflector.MergeOptions.KEEP_TARGET);

            merged = Wrapper.Article.CreateArticle(merged);

            Share newShare = new Share()
            {
                WriteDate = DateTime.Now,
                Slug = article.Slug,
                VDepth = 0,
                
                BlogId = article.BlogId,
                SharingBlogId = merged.BlogId,
            };

            Create(newShare);
            TrySave();

            Wrapper.Notificaton.CreateAShare(newShare);

            return getShare(newShare);
        }
    }
}
