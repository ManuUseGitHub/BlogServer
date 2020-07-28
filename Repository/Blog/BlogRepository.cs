using Entities;
using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Repository
{
    public partial class BlogRepository : EmmBlogRepositoryBase<Blog>, IBlogRepository
    {
        public BlogRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        public Subscribe CreateSubscription(string mailAddress, Guid? blogId)
        {
            return CreateSubscription(Wrapper
                .Account
                .GetAccountByMailAddress(mailAddress)
                .Id,blogId
            );
        }

        public Collection<Account> GetAllowedAccounts(Account account)
        {
            return GetAllowedAccounts(Wrapper
                .Blog
                .GetBlogOfAccount(account.Id)
            );
        }

        public Collection<Account> GetAllowedAccounts(Blog blog)
        {
            Collection<Account> allowed = new Collection<Account>();

            HashSet<Guid?> set = Wrapper
                .Follow
                .GetBlockingOf(blog.Account);

            ICollection<Subscribe> allSubscribers = blog.Subscribe;
            
            if(allSubscribers != null)
            {
                foreach (Subscribe sub in allSubscribers)
                {
                    if (!set.Contains(sub.AccountId))
                    {
                        allowed.Add(sub.Account);
                    }
                }
            }

            return allowed;
        }

        private void RemoveFromArticlesFromResultForNonFriend(
            Blog blog, Guid? idViewer
        )
        {
            Follow canSee = Wrapper.Follow.GetFollow(new Follow()
            {
                FollowedId = blog.AccountId,
                FollowingId = idViewer
            });

            // Not friend yet
            if (canSee == null || canSee.StatusId == F_FOLLOW)
            {
                blog.Articles = blog.Articles
                    .Where(a => a.VisibilityId.Equals(PUBLIC))
                    .ToList();
            }
            else
            {
                blog.Articles = blog.Articles;
            }
        }
    }
}