using Contracts;
using Entities.EmmBlog.DataModelObjects;
using System;
using Utilities;

namespace Repository
{
    public partial class FollowRepository : EmmBlogRepositoryBase<Follow>, IFollowRepository
    {
        public Follow CreateAFollow(Follow follow)
        {
            StatusStateMachine<Follow>.FollowAccount(GetFollow(follow));

            Follow newFollow = NewFollowModel(follow, F_FOLLOW);

            Create(newFollow);
            TrySave();

            Follow result = GetFollow(newFollow);


            return result;
        }

        private Follow NewFollowModel(Follow follow,int? statusId )
        {
            Follow newFollow = new Follow()
            {
                FollowingDate = DateTime.Now,
                FollowedId = follow.Followed.Id,
                FollowingId = follow.Following.Id,

                StatusId = statusId,
                ImportanceId = DEFAULT
            };

            Reflector.Merge(newFollow, follow);
            return newFollow;
        }

        public void ClaimAFriendShip(Follow follow)
        {
            Follow found = GetFollow(follow);
            Follow result;

            StatusStateMachine<Follow>.ClaimFsRequest(found);

            // get the good pending depending on the existance of a follow
            int pending = found == null ? 
                F_PENDINGA : 
                F_PENDINGB;

            if(pending == F_PENDINGA)
            {
                Follow newFollow = NewFollowModel(follow, pending);
                Create(newFollow);

                TrySave();
                result = GetFollow(newFollow);
            }
            else 
            {
                found.StatusId = F_PENDINGB;
                Update(found);

                TrySave();
                result = GetFollow(found);
            }

            Wrapper.Notificaton.CreateAFriendRequest(result);
        }

        public void CreateAFriendShip(Follow follow)
        {
            Follow Reciprocal = GetFreshReciprocal(follow);

            Follow found = GetFollow(Reciprocal); // should be null => unexistent;
            StatusStateMachine<Follow>.CreateFriendship(found);

            Create(Reciprocal);
            TrySave();
        }

        public Follow MakeABlock(Follow blocking)
        {
            Follow found = GetFollow(blocking);
            
            StatusStateMachine<Follow>.BlockAccount(found);

            if (found != null)
            {
                found.IsBlocking = true;
                Update(found);
            }
            else
            {

                Follow newFollow = new Follow()
                {
                    FollowingDate = DateTime.Now,
                    FollowedId = blocking.Followed.Id,
                    FollowingId = blocking.Following.Id,

                    StatusId = F_BLOCKED,
                    ImportanceId = DEFAULT
                };
                Reflector.Merge(newFollow, blocking);

                StatusStateMachine<Follow>.BlockAccount(found);

                newFollow.IsBlocking = true;
                Create(newFollow);
            }

            TrySave();

            Follow result = GetFollow(blocking);
            return result;
        }

        public Follow RemoveABlock(Follow blocking)
        {
            Follow found = GetFollow(blocking);
            StatusStateMachine<Follow>.AllowAccount(found);

            if (found.StatusId == F_BLOCKED)
            {
                Delete(found);
                return null;
            }
            else
            {
                found.IsBlocking = false;
                Update(found);
            }

            TrySave();

            return GetFollow(blocking);
        }
    }
}