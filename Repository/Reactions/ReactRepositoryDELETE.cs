using Contracts;
using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Repository
{
    public partial class ReactRepository : EmmBlogRepositoryBase<React>, IReactRepository
    {

        public React RemoveReaction<T>(T entity, Account account)
        {
            React removed = GetReact(entity, account);
            Delete(removed);
            TrySave();

            return removed;
        }

        public void RemoveReactionsOf<T>(T entity)
        {
            if (entity is Comment)
            {
                Comment comment = entity as Comment;

                RemoveRelatedReactions(r =>
                    r.CommentId.Equals(comment.Id)
                );
            }
            else if (entity is Article)
            {
                Article article = entity as Article;

                RemoveRelatedReactions(r =>
                    r.Slug.Equals(article.Slug) &&
                    r.BlogId.Equals(article.BlogId)
                );
            }
        }

        private void RemoveRelatedReactions( Expression<Func<React, bool>> expression) 
        {
            ICollection<React> removables = FindByCondition(expression)
                .ToList();

            foreach (React react in removables)
            {
                Delete(react);
            }

            TrySave();
        }
    }
}