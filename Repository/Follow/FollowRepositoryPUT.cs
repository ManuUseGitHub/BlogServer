using Contracts;
using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class FollowRepository : EmmBlogRepositoryBase<Follow>, IFollowRepository
    {
        public void AcceptFriend(Follow follow)
        {
            Follow found = GetFollow(follow);
            StatusStateMachine<Follow>.AcceptFriendShip(found);

            found.StatusId = F_FRIEND;
            Wrapper.Notificaton.CreateAcceptedFriendship(found);

            Update(found);
            TrySave();

            CreateAFriendShip(follow);
        }

        public void UndoFriendShipRequest(Follow found)
        {
            found.StatusId = F_FOLLOW;
            TrySave();
        }
    }
}