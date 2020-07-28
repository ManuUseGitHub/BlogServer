using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using Entities.EmmBlog.DataModelObjects.Utilities;
using Repository.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using static Contracts.IRecordKind;
using static Repository.Exceptions.CommentsExceptions;

namespace Repository
{
    public partial class CommentRepository : EmmBlogRepositoryBase<Comment>, ICommentRepository
    {
        public Comment GetComment(Comment comment, RecordKind kind = RecordKind.UPTODATE)
        {
            if (kind == RecordKind.UPTODATE)
            {
                comment.RDepth = 0;
            }
            else if (kind == RecordKind.OLDEST)
            {
                // depth >= 0
                SetMostOldVersion(comment);
            }
            else if (kind == RecordKind.DEFINED)
            {
                // TODO code for defined case
            }

            var fetched = FindByCondition(c =>
                    c.Id.Equals(comment.Id) &&
                    c.RDepth.Equals(comment.RDepth)
                )
                .FirstOrDefault();

            if (fetched == null)
            {
                DbEx.Instance.Throw<NoCommentFoundException>(comment);
            }

            Wrapper.React.SetTop3ReactionCounts(fetched);
            return fetched;
        }

        public ICollection<Comment> GetMostUpToDateCommentList(Article article)
        {
            ICollection<Comment> list = new List<Comment>();

            ICollection<Guid?> comments = article.Comments
                .Select(c => c.Id)
                .ToList();

            var commentKeys = new HashSet<Guid?>(comments);
            foreach (Guid k in commentKeys)
            {
                list.Add(FindByCondition(c => c.Id.Equals(k))
                    .OrderByDescending(c => c.RDepth)
                    .FirstOrDefault());
            }

            return list;
        }

        public List<IWritableDate> GetCommentRevisions(Comment comment)
        {
            IQueryable<IWritableDate> query = RepositoryContext.Comments
                .Where(c => c.Id.Equals(comment.Id));

            return DataPresenter.GetSortedByWriteDate(query.ToList()).ToList();
        }
    }
}