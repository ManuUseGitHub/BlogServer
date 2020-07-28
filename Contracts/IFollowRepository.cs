using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface IFollowRepository : IRepositoryBase<Follow>
    {
        Follow CreateAFollow(Follow friendShip);
        public Follow GetFollow(Follow follow);
        void AcceptFriend(Follow follow);
        Follow MakeABlock(Follow blocking);
        ICollection<Follow> GetBlockedBy(Account followed);
        Follow RemoveABlock(Follow blocking);
        void CreateAFriendShip(Follow follow);
        void ClaimAFriendShip(Follow friendship);
        void CancelFriendShipRequest(Follow friendship);
        void RemoveAFollow(Follow friendship);
        void RemoveFriendship(Follow friendship);
        public HashSet<Guid?> GetBlockingOf(Account account);
        public HashSet<Guid?> GetBlockingOf(Guid? accountId);
        public ICollection<Account> GetFriendList(Guid? accountId);
    }
}
