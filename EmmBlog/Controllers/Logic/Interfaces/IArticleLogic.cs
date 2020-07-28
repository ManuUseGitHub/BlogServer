using Microsoft.AspNetCore.Mvc;
using System;

namespace EmmBlog.Controllers
{
    public partial class ArticleController
    {
        internal interface IArticleLogic
        {
            IActionResult GetArticleOfSlug<DTO>(string id);

            IActionResult GetArticlesOfBlog<DTO>(Guid id);
        }
    }
}