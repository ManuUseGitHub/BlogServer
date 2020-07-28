using Contracts;
using Entities.EmmBlog.DataModelObjects;
using Utilities;
using static Entities.EmmBlog.DataModelObjects.Status;

namespace Repository
{
    public partial class FollowRepository : EmmBlogRepositoryBase<Follow>, IFollowRepository
    {
        public void RemoveAFollow(Follow friendship)
        {

            Follow found = GetFollow(friendship);
            StatusStateMachine<Follow>.StopFollowingAccount(found);

            //TODO: add custom code to handle this case

            Delete(found);
            TrySave();
        }

        public void DeleteFriendShipRequest(Follow found)
        {
            Delete(found);
            TrySave();
        }

        public void RemoveFriendship(Follow friendship)
        {
            foreach (Follow transiant in new Follow[]{
                friendship,
                GetFreshReciprocal(friendship)
            }) {
                Follow found = GetFollow(transiant);
                StatusStateMachine<Follow>.RemoveFriendship(found);
                
                Delete(found);
                TrySave();
            }
        }
    }
}
