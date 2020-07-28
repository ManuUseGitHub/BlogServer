using AutoMapper;
using Contracts;
using Entities.EmmBlog.DataTransferObjects.Out.Reduced;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EmmBlog.Controllers
{
    [Route("api.net")]
    [ApiController]
    public partial class ArticleController : DBController
    {
        private IArticleLogic ArticleLgc { get; }

        public ArticleController(

            ILoggerManager logger,
            IRepositoryWrapper repository,
            IMapper mapper,
            IRepoLoaderWrapper repoLoader

        ) : base(logger, repository, mapper, repoLoader)
        {
            ArticleLgc = new ArticleLogic();
            (ArticleLgc as ArticleLogic).Ctrl = this;
        }

        [HttpGet]
        [Route("article/slug/{slug}")]
        public IActionResult GetArticleOfSlug(string slug)
        {
            return HandleOnDTO(slug, ArticleLgc.GetArticleOfSlug<ArticlesOfBlogDTO>);
        }

        [HttpGet]
        [Route("articles/of/{id}")]
        public IActionResult GetArticlesOfBlog(Guid id)
        {
            return HandleOnId(id, ArticleLgc.GetArticlesOfBlog<ICollection<PureArticleDTO>>);
        }
    }
}