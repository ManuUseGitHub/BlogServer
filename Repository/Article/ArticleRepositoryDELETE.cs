using Contracts;
using Entities.EmmBlog.DataModelObjects;
using Utilities;
using Repository.Exceptions;
using System;
using System.Linq;
using static Utilities.Reflector;
using static Repository.Exceptions.ArticlesExceptions;

namespace Repository
{
    public partial class ArticleRepository : EmmBlogRepositoryBase<Article>, IArticleRepository
    {
        public Article RemoveArticle(Article article)
        {
            var articles = FindByCondition(a =>
                a.Slug.Equals(article.Slug) &&
                a.BlogId.Equals(article.BlogId)
                ).OrderByDescending(a => a.VDepth)
                .ToList();
            
            Article copy = Util.GetCopyOf(article);

            try
            {
                Article stored = Wrapper.Article.FindByCondition(c =>
                    c.Slug.Equals(article.Slug) &&
                    c.BlogId.Equals(article.BlogId))
                    .FirstOrDefault();

                if (stored.Shares != null || stored.Comments != null)
                {
                    DbEx.Instance
                        .Throw<NoReferencedArticleCanBeDeletedException>(stored);
                }

                foreach (Article deletable in articles)
                {
                    // better to remove reactions first even if it is automated (cascading)
                    Wrapper.React.RemoveReactionsOf(deletable);

                    // then remove the deletable commment
                    Delete(deletable);
                }
            }
            catch (Exception ex)
            {
                // set to removed instead of deleting
                foreach (Article deletable in articles)
                {
                    // better to remove reactions first even if it is automated (cascading)
                    Wrapper.React.RemoveReactionsOf(deletable);

                    // set the visibility to removed instead of deleting every version
                    // of the original
                    deletable.VisibilityId = REMOVED;
                }
            }
            TrySave();

            return copy;
        }

        public Article RemoveArticle(Share share)
        {
            Article search = new Article()
            {
                BlogId = share.SharingBlogId
            };

            Reflector.Merge(search, share.Article, MergeOptions.KEEP_TARGET);

            return RemoveArticle(search);
        }
    }
}