using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using Utilities;
using Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Contracts.IRecordKind;
using static Repository.Exceptions.ArticlesExceptions;

namespace Repository
{
    public partial class ArticleRepository : EmmBlogRepositoryBase<Article>, IArticleRepository
    {
        public Article GetArticleOfSlug(string slug)
        {
            var article = RepositoryContext.Article
                .Where(a => a.Slug.Equals(slug))
                .FirstOrDefault();

            return article;
        }

        public ICollection<Article> GetArticlesOfBlog(Guid blogGuid)
        {
            var blog = Wrapper.Blog.GetBlogArticles(blogGuid);
            return blog != null ? blog.Articles : null;
        }

        public Article GetArticle(Article article, RecordKind kind = RecordKind.UPTODATE)
        {
            if (kind == RecordKind.UPTODATE)
            {
                article.VDepth = 0;
            }
            else if (kind == RecordKind.OLDEST)
            {
                // depth >= 0
                SetMostOldVersion(article);
            }
            else if (kind == RecordKind.DEFINED)
            {
                // TODO code for defined case
            }

            var fetched = FetchArticle(article);


            if (fetched == null)
            {
                DbEx.Instance.Throw<NoArticleFoundException>(article);
            }

            Wrapper.React.SetTop3ReactionCounts(fetched);
            return fetched;
        }

        /// <summary>
        /// Article which has it's most uptodate comments. Do not use the result in a process
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public Article GetEndArticle(Article article)
        {
            Article copy = Reflector.ReferShalowedCopy(GetArticle(article));

            if (copy.Comments != null)
            {
                copy.Comments = Wrapper.Comment.GetMostUpToDateCommentList(copy);
            }

            return copy;
        }

        public List<Article> GetThreadOfArticle(Article article)
        {
            return FindByCondition(a =>
                a.Slug.Equals(article.Slug) &&
                a.BlogId.Equals(article.BlogId)
            ).OrderByDescending(a => a.VDepth)
                .ToList();
        }

        private Article FetchArticle(Article article)
        {
            return FindByCondition(a =>
                a.VDepth.Equals(article.VDepth) &&
                a.BlogId.Equals(article.BlogId) &&
                a.Slug.Equals(article.Slug)
            ).FirstOrDefault();
        }
    }
}