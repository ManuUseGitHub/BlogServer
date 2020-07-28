using Entities;
using Entities.EmmBlog.DataModelObjects;
using Repository.Exceptions;
using System;
using System.Linq;
using static Repository.Exceptions.BlogsExceptions;

namespace Repository
{
    public partial class BlogRepository : EmmBlogRepositoryBase<Blog>, IBlogRepository
    {
        public Blog CreateBlog(Blog blog)
        {
            Create(blog);
            blog.VisibilityId = PUBLIC;

            TrySave();

            Blog created = FindByCondition(b => b.Id.Equals(blog.Id)).FirstOrDefault();

            if (created == null)
            {
                DbEx.Instance.Throw<NotFoundBlogException>();
            }
            return created;
        }

        public Subscribe CreateSubscription(Account account, Guid? blogId)
        {
            return CreateSubscription(account.Id, blogId);
        }

        public Subscribe CreateSubscription(Guid? accountId, Guid? blogId)
        {
            Subscribe subscription = new Subscribe();
            subscription.AccountId = accountId;
            subscription.BlogId = blogId;
            subscription.WriteDate = DateTime.Now;

            Create(subscription);
            TrySave();

            Subscribe found = RepositoryContext.Subscribe
                .Where(s => blogId.Equals(s.BlogId) && accountId.Equals(s.AccountId))
                .FirstOrDefault();

            if (found == null)
            {
                DbEx.Instance.Throw<NoMatchingSubscriptionException>();
            }

            return found;
        }
    }
}