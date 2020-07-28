using Entities;
using Entities.EmmBlog.DataModelObjects;
using Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Utilities;
using static Repository.Exceptions.BlogsExceptions;

namespace Repository
{
    public partial class BlogRepository : EmmBlogRepositoryBase<Blog>, IBlogRepository
    {

        public ICollection<Blog> GetBlogs()
        {
            return FindAll().ToList();
        }

        public Blog GetBlogById(Guid? id)
        {
            var blog = FindByCondition(a => a.Id.Equals(id))
                .FirstOrDefault();

            return blog;
        }

        public Blog GetBlogArticles(Guid? id)
        {
            var blog = FindByCondition(a => a.Id.Equals(id))
                .FirstOrDefault();

            return blog;
        }

        public Blog GetBlogOfAccount(Guid? id)
        {
            Blog found = FindByCondition(b => b.AccountId.Equals(id))
                .AsNoTracking()
                .FirstOrDefault();

            if (found == null)
            {
                DbEx.Instance.Throw<NotFoundBlogException>();
            }

            return found;
        }

        public Blog GetEndBlogOfAccount(Guid? id, Guid? idViewer)
        {   
            Blog copy = Util.GetCopyOf(GetBlogOfAccount(id));

            RemoveFromArticlesFromResultForNonFriend(copy, idViewer);

            return copy;
        }
    }
}