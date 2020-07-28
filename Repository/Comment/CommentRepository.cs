using Entities;
using Entities.EmmBlog.DataModelObjects;
using System.Linq;

namespace Repository
{
    public partial class CommentRepository : EmmBlogRepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        private void SetMostOldVersion(Comment comment)
        {
            Comment mostUpdated = FindByCondition(c => c.Id.Equals(comment.Id))
                .ToList()
                .OrderByDescending(c => c.RDepth)
                .FirstOrDefault();

            if (mostUpdated != null)
            {
                comment.RDepth = mostUpdated.RDepth;
            }
        }
    }
}