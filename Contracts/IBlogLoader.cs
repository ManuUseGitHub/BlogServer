using Entities.EmmBlog.DataModelObjects;
using System;

namespace Contracts
{
    public interface IBlogLoader
    {
        void LoadBlogs(Blog blog);
        void LoadBlogArticles(Blog blog);
        void LoadBlogSubscriptions(Blog blog);
        void LoadBlogVisibility(Blog blog);
    }
}