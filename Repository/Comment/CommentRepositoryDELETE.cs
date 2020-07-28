using Entities.EmmBlog.DataModelObjects;
using Utilities;
using Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Repository.Exceptions.CommentsExceptions;

namespace Repository
{
    public partial class CommentRepository : EmmBlogRepositoryBase<Comment>, ICommentRepository
    {
        public Comment RemoveComment(Comment comment)
        {
            Comment copy = Util.GetCopyOf(comment);

            Comment answer = FindByCondition(c => c.AnswerId.Equals(comment.Id))
                    .FirstOrDefault();

            var comments = FindByCondition(c => c.Id.Equals(comment.Id))
                .ToList();

            try
            {
                StabilityCheck(answer);
                RemoveUndependent(comments);
            }
            catch (Exception ex)
            {
                RemoveWithDepend(comments);
            }
            TrySave();

            return copy; ;
        }

        private void RemoveUndependent(List<Comment> comments)
        {
            var sorted = comments
                .OrderByDescending(c => c.RDepth)
                .ToList();

            foreach (Comment deletable in sorted)
            {
                // better to remove reactions first even if it is automated (cascading)
                Wrapper.React.RemoveReactionsOf(deletable);

                // then remove the deletable commment
                Delete(deletable);
            }
        }

        private void RemoveWithDepend(List<Comment> comments)
        {
            // set to removed instead of deleting
            foreach (Comment deletable in comments)
            {
                // set the visibility to removed instead of deleting every version
                // of the original comment
                deletable.VisibilityId = REMOVED;

                Wrapper.React.RemoveReactionsOf(deletable);
            }
        }

        private void StabilityCheck(Comment answer)
        {
            if (answer != null)
            {
                DbEx.Instance.Throw<NoAnsweredCommentCanBeDeletedException>(answer);
            }
        }
    }
}