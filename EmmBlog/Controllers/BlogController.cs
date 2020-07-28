using AutoMapper;
using Contracts;
using EmmBlog.Controllers.Logic.Implementations;
using EmmBlog.Controllers.Logic.Interfaces;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataTransferObjects;
using Entities.EmmBlog.DataTransferObjects.Out.Reduced;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace EmmBlog.Controllers
{
    [Route("api.net/blog")]
    [ApiController]
    public class BlogController : DBController
    {
        private IBlogLogic BlogLgc { get; set; }

        public BlogController(

            ILoggerManager logger, 
            IRepositoryWrapper repository, 
            IMapper mapper, 
            IRepoLoaderWrapper repoLoader

        ): base(logger, repository, mapper, repoLoader)
        {
            BlogLgc = new BlogLogic();
            (BlogLgc as BlogLogic).Ctrl = this;
        }

        [HttpGet("{id}")]
        public IActionResult _Ok(Guid id)
        {
            return HandleOnId(id, BlogLgc.GetBlogById<BlogWithSubscription>);
        }

        [HttpGet("{id}/articles")]
        public IActionResult WithArticles(Guid id)
        {
            return HandleOnId(id, BlogLgc.GetBlogArticles<BlogWithArticlesDTO>);
        }

        [HttpGet("all")]
        public IActionResult GetAllBlogs()
        {
            return HandleNoParameter(BlogLgc.GetBlogs<List<ListedBlogDTO>>);
        }
    }
}