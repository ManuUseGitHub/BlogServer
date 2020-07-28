using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using Utilities;
using Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static Utilities.Reflector;
using static Repository.Exceptions.ArticlesExceptions;

namespace Repository
{
    public partial class ArticleRepository : EmmBlogRepositoryBase<Article>, IArticleRepository
    {
        public ArticleRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        public void AssignCommentsToArticle(Article article)
        {
            var leadingComments = RepositoryContext.Comments
                .Where(c => c.Slug.Equals(article.Slug) && c.AnswerId == null);
        }

        private void SuggestSlug(Article article)
        {
            if (article.Slug == null || article.Slug.Length == 0)
            {
                article.Slug = Util.GetSlugBasedOnString(article.Title);
            }
        }

        private void SetMostOldVersion(Article article)
        {
            Article mostUpdated = FindByCondition(a =>
                    a.BlogId.Equals(article.BlogId) &&
                    a.Slug.Equals(article.Slug)
                ).ToList()
                .OrderByDescending(a => a.VDepth)
                .FirstOrDefault();

            if (mostUpdated != null)
            {
                article.VDepth = mostUpdated.VDepth;
            }
        }

        private void UdateArchieve(Article changed)
        {
            string? slug = changed.Slug;
            Guid? blogId = changed.BlogId;

            var archieve = FindByCondition(a =>
               a.Slug.Equals(slug) &&
               a.BlogId.Equals(blogId) &&
               a.VDepth > 0)
                .OrderByDescending(a => a.VDepth)
                .ToList();

            foreach (Article deeper in archieve)
            {
                Article article = Util.GetCopyOf(FindByCondition(a =>
                    a.Slug.Equals(deeper.Slug) &&
                    a.BlogId.Equals(deeper.BlogId) &&
                    a.VDepth.Equals(deeper.VDepth - 1)
                ).FirstOrDefault());

                // remove dependencies

                Reflector.Merge(deeper, article,

                    // default keep source value when collision
                    MergeOptions.KEEP_SOURCE,

                    // else apply the inversed rule for these as KEEP_TARGET
                    // these fields tend ti attach the entity to existing in
                    // the database. It can lead to dependencies problems otherwise
                    new List<PropertyInfo>() {
                        GetPInfo(article, a => article.Shares),
                        GetPInfo(article, a => article.Comments),
                        GetPInfo(article, a => article.VDepth)
                    });

                TrySave();
            }
        }

        private void ThrowOnExisting(Article article)
        {
            Article found = FetchArticle(article);

            if (found != null)
            {
                DbEx.Instance.Throw<TwiceAddingArticleException>(found);
            }
        }
    }
}