using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Repository.Exceptions;
using System;
using System.Linq;
using Utilities;
using static Contracts.INotificationRepository;
using static Repository.Exceptions.BlogsExceptions;

namespace Repository
{
    public partial class BlogRepository : EmmBlogRepositoryBase<Blog>, IBlogRepository
    {
        public void ChangeBlogInfos(Blog blog)
        {
            Blog retrieved = GetBlogOfAccount(blog.AccountId);
            Blog rCopy = Util.GetCopyOf(retrieved);

            Reflector.Merge(retrieved, blog);

            Update(retrieved);
            TrySave();

            if (!rCopy.BlogName.Equals(blog.BlogName))
            {
                Wrapper.Notificaton.NotifyAllowedAccountOf(retrieved, NotificationTopic.BLOG_NAME);
            }
        }
    }
}