using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Repository
{
    public partial class ReactRepository : EmmBlogRepositoryBase<React>, IReactRepository
    {
        public ReactRepository(
            RepositoryWrapper repositoryWrapper,
            RepositoryContext repositoryContext)
            : base(repositoryWrapper, repositoryContext)
        { }

        private void SetReferenceKeys(React reaction)
        {
            IHaveReactions reactible =
                reaction.Article != null ?
                (IHaveReactions)reaction.Article :
                reaction.Comment;

            // AccountId
            reaction.AccountId = reaction.Account.Id;

            // TypeId
            reaction.TypeId = reaction.Reaction.TypeId;
            reaction.Reaction = null;

            // Slug, BlogId, Version
            if (reactible is Article)
            {
                SetArticleReferenceKey(reaction);
                //reaction.Article = null;
            }
            // CommentId, Revision
            else if (reactible is Comment)
            {
                SetCommentRefenceKeys(reaction);
            }
        }

        private void SetCommentRefenceKeys(React reaction)
        {
            Comment c = reaction.Comment as Comment;
            
            c = Wrapper.Comment.GetComment(c);

            reaction.ItemId = new StringBuilder()
                .Append(c.Id).Append("|")
                .ToString();

            reaction.CommentId = c.Id;
            reaction.RDepth = c.RDepth;
        }

        private void SetArticleReferenceKey(React reaction)
        {
            Article a = reaction.Article as Article;

            reaction.ItemId = new StringBuilder()
                .Append(a.BlogId).Append("|")
                .Append(a.Slug)
                .ToString();

            reaction.Slug = a.Slug;
            reaction.VDepth = a.VDepth;
            reaction.BlogId = a.BlogId;
        }

        public void SetTop3ReactionCounts(IHaveReactions item)
        {
            var reactions = new List<ReactionCounter>();

            if (item.Reactions != null)
            {
                var reactionTypes = RepositoryContext.ReactionTypes.ToList();


                foreach (ReactionType rt in reactionTypes)
                {
                    // Create a counter for each reaction type
                    ReactionCounter counter = new ReactionCounter()
                    {
                        RType = rt,
                        Count = item.Reactions
                            .Count(i => i.TypeId.Equals(rt.TypeId))
                    };

                    // Add all counts of reaction types
                    reactions.Add(counter);
                }
            }

            // Assign top 3 of top reactions
            item.ReactionCounts = reactions
                .OrderByDescending(r => r.Count)
                .Take(3)
                .ToList();
        }
    }
}