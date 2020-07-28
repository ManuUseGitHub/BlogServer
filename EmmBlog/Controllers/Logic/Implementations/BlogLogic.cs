using EmmBlog.Controllers.Logic.Interfaces;
using Entities.EmmBlog.DataModelObjects;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace EmmBlog.Controllers.Logic.Implementations
{
    public class BlogLogic : DBLogic, IBlogLogic
    {
        public IActionResult GetBlogArticles<DTO>(Guid id)
        {
            var blog = Ctrl.Repository.Blog.GetBlogArticles(id);

            if (blog == null)
            {
                Ctrl.Logger.LogError($"Account with id: {id}, hasn't been found in db.");
                return Ctrl.NotFound();
            }
            else
            {
                Ctrl.Logger.LogInfo($"Returned acccount with id: {id}");

                // Loading of articles
                Ctrl.RepoLoader.Blog.LoadBlogArticles(blog);

                var ownerResult = Ctrl.Mapper.Map<DTO>(blog);
                return Ctrl.Ok(ownerResult);
            }
        }

        public IActionResult GetBlogById<DTO>(Guid id)
        {
            var blog = Ctrl.Repository.Blog.GetBlogById(id);
            Ctrl.RepoLoader.Blog.LoadBlogs(blog);

            if (blog == null)
            {
                Ctrl.Logger.LogError($"Account with id: {id}, hasn't been found in db.");
                return Ctrl.NotFound();
            }
            else
            {
                Ctrl.Logger.LogInfo($"Returned acccount with id: {id}");

                // Loading of articles
                Ctrl.RepoLoader.Blog.LoadBlogArticles(blog);
                Ctrl.RepoLoader.Blog.LoadBlogSubscriptions(blog);
                foreach (Subscribe subscribe in blog.Subscribe)
                {
                    Ctrl.RepoLoader.Account.LoadAccount(subscribe);
                    Ctrl.RepoLoader.Account.LoadCredentials(subscribe.Account);
                }
                Ctrl.RepoLoader.Account.LoadAccount(blog);
                Ctrl.RepoLoader.Account.LoadCredentials(blog.Account);

                var ownerResult = Ctrl.Mapper.Map<DTO>(blog);
                return Ctrl.Ok(ownerResult);
            }
        }

        public IActionResult GetBlogs<DTO>()
        {
            ICollection<Blog> blogs = Ctrl.Repository.Blog.GetBlogs();
            ICollection<Blog> loaddedBlogs = new List<Blog>();


            foreach (Blog blog in blogs)
            {
                loaddedBlogs.Add(Ctrl.Repository.Blog.GetBlogById(blog.Id));
            }

            foreach (Blog blog in loaddedBlogs)
            {
                Ctrl.RepoLoader.Blog.LoadBlogVisibility(blog);
                Ctrl.RepoLoader.Blog.LoadBlogArticles(blog);
                Ctrl.RepoLoader.Account.LoadAccount(blog);
                Ctrl.RepoLoader.Blog.LoadBlogSubscriptions(blog);
            }
            

            var ownerResult = Ctrl.Mapper.Map<DTO>(loaddedBlogs);
                return Ctrl.Ok(ownerResult);
            
        }
    }
}