using EmmBlog.Controllers.Logic;
using Entities.EmmBlog.DataModelObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EmmBlog.Controllers
{
    public partial class ArticleController
    {
        internal class ArticleLogic : DBLogic, IArticleLogic
        {
            public ArticleLogic()
            {
            }

            public IActionResult GetArticlesOfBlog<DTO>(Guid id)
            {
                var articles = Ctrl.Repository.Article.GetArticlesOfBlog(id);
                
                return ResultOrNotFound<ICollection<Article>, DTO>(articles, id);
            }

            public IActionResult GetArticleOfSlug<DTO>(string slug)
            {
                var article = Ctrl.Repository.Article.GetArticleOfSlug(slug);

                if (article == null)
                {
                    Ctrl.Logger.LogError($"Account with id: {slug}, hasn't been found in db.");
                    return Ctrl.NotFound();
                }
                else
                {
                    Ctrl.Logger.LogInfo($"Returned acccount with id: {slug}");

                    Ctrl.RepoLoader.Article.LoadBlogOfArticle(article);
                    Ctrl.RepoLoader.Article.LoadAccount(article);
                    article.Revisions = Ctrl.Repository.Article.GetThreadOfArticle(article);

                    var result = Ctrl.Mapper.Map<DTO>(article);
                    return Ctrl.Ok(result);
                }
            }
        }
    }
}