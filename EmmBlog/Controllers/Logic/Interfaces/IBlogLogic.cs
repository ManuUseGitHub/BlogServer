using Microsoft.AspNetCore.Mvc;
using System;

namespace EmmBlog.Controllers.Logic.Interfaces
{
    public interface IBlogLogic
    {
        IActionResult GetBlogById<DTO>(Guid id);

        IActionResult GetBlogArticles<DTO>(Guid id);

        IActionResult GetBlogs<DTO>();
    }
}