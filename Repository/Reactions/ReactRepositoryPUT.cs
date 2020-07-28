using Contracts;
using Entities.EmmBlog.DataModelObjects;
using System.Collections.Generic;
using System.Linq;

namespace Repository
{
    public partial class ReactRepository : EmmBlogRepositoryBase<React>, IReactRepository
    {
        public void UpgradReactionsOf<T>(T entity)
        {
            ICollection<React> reactions = null;
            if (entity is Comment)
            {
                UpgradeCommentReactions(out reactions, entity as Comment);
            }
            else if (entity is Article)
            {
                UpgradeArticleReactions(out reactions, entity as Article);
            }
            if (reactions.Count > 0)
            {
                TrySave();
            }
        }

        private void UpgradeArticleReactions(out ICollection<React> reactions, Article copy)
        {
            // reactions to an article (slug + blogId)
            reactions = FindByCondition(r =>
                r.Slug.Equals(copy.Slug) &&
                r.BlogId.Equals(copy.BlogId)
            ).ToList();

            // Change the version of matching reactions
            foreach (React reaction in reactions)
            {
                reaction.VDepth = copy.VDepth;
            }
        }

        private void UpgradeCommentReactions(out ICollection<React> reactions, Comment copy)
        {
            // reactions to a comment (commentId)
            reactions = FindByCondition(r =>
                r.CommentId.Equals(copy.Id)
            ).ToList();

            // Change the revision of matching reactions
            foreach (React reaction in reactions)
            {
                reaction.RDepth = copy.RDepth;
            }
        }
    }
}