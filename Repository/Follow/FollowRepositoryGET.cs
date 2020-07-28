using Contracts;
using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class FollowRepository : EmmBlogRepositoryBase<Follow>, IFollowRepository
    {

        public Follow GetFollow(Follow follow)
        {
            if (follow.Followed != null)
            {
                follow.FollowedId = follow.Followed.Id;
            }

            if (follow.Following != null)
            {
                follow.FollowingId = follow.Following.Id;
            }

            return FindByCondition(f =>
                f.FollowedId.Equals(follow.FollowedId) &&
                f.FollowingId.Equals(follow.FollowingId)
            ).FirstOrDefault();
        }

        public ICollection<Follow> GetBlockedBy(Account account)
        {
            return FindByCondition(f =>
                f.FollowedId.Equals(account.Id) &&
                f.IsBlocking
            ).ToList();
        }

        public HashSet<Guid?> GetBlockingOf(Guid? accountId)
        {
            HashSet<Guid?> rresult = new HashSet<Guid?>();

            List<Follow> allBlockingForAccount = FindByCondition(f =>
                 f.IsBlocking && (
                     f.FollowedId.Equals(accountId) ||
                     f.FollowingId.Equals(accountId)
                 )
            ).ToList();

            foreach (Follow f in allBlockingForAccount)
            {

                if (f.FollowedId != accountId)
                {
                    rresult.Add(f.FollowedId);
                }

                else
                {
                    rresult.Add(f.FollowingId);
                }

            }

            return rresult;
        }


        public HashSet<Guid?> GetBlockingOf(Account account)
        {
            return GetBlockingOf(account.Id);
        }

        public ICollection<Account> GetFriendList(Guid? accountId)
        {
            List<Account> friends = new List<Account>();

            List<Follow> allNonBlockingForAccount = FindByCondition(f =>
                 f.FollowingId.Equals(accountId) && !f.IsBlocking
            ).ToList();

            foreach (Follow allowed in allNonBlockingForAccount)
            {
                friends.Add(allowed.Followed);
            }

            return friends;
        }
    }
}
