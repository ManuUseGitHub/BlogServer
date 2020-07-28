using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Exceptions
{
    public class BlogsExceptions : EmmBlogException<Blog>
    {
        public class NotFoundBlogException : NullReferenceException
        {
            public NotFoundBlogException(string message) : base(message)
            { }

            public NotFoundBlogException(Blog blog) : base (
                ExpliciteNotFoundMessage(blog, new object[] {blog.Id}))
            { }
        }

        public class NoMatchingSubscriptionException : NullReferenceException
        {
            public NoMatchingSubscriptionException(string message) : base(message)
            { }
        }
    }
}
