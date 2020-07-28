using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Utilities;

namespace Repository
{
    public partial class FollowRepository : EmmBlogRepositoryBase<Follow>, IFollowRepository
    {
        public FollowRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        public void CancelFriendShipRequest(Follow friendship)
        {
            Follow found = GetFollow(friendship);
            StatusStateMachine<Follow>.CancelFsRequest(found);

            if (found.StatusId == F_PENDINGA)
            {
                DeleteFriendShipRequest(found);
            }
            else if (found.StatusId == F_PENDINGB)
            {
                UndoFriendShipRequest(found);
            }
        }

        private Follow GetFreshReciprocal(Follow follow)
        {
            Follow copy = Util.GetCopyOf(follow);

            // the reciproquity
            Follow fresReciprocal = new NullNormalizeFactory<Follow>(f => {
                f.Followed = copy.Following;
                f.Following = copy.Followed;

                f.StatusId = F_FRIEND;
                f.ImportanceId = DEFAULT;
            }).Instance;


            // remove references from the copy so they will be skipped from the 
            // merging process
            copy.Followed = copy.Following = null;

            Reflector.Merge(fresReciprocal, copy);

            return fresReciprocal;
        }
    }
}
