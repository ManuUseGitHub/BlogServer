using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using static Contracts.IRecordKind;

namespace Contracts
{
    public interface IArticleRepository : IRepositoryBase<Article>
    {
        Article GetArticleOfSlug(string slug);
        public void AssignCommentsToArticle(Article article);
        ICollection<Article> GetArticlesOfBlog(Guid blogGuid);
        Article CreateArticle(Article article);
        public Article GetArticle(Article article, RecordKind kind = RecordKind.UPTODATE);
        Article GetEndArticle(Article article);
        Article UpdateArticle(Article copy);
        List<Article> GetThreadOfArticle(Article article);
        Article RemoveArticle(Article published);
        Article RemoveArticle(Share shared);
    }
}