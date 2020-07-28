using Contracts;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System.Collections.Generic;
using static Contracts.IRecordKind;

namespace Repository
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        Comment CommentArticle(Comment comment);

        Comment AnswerComment(Comment comment);

        Comment GetComment(Comment comment, RecordKind kind = RecordKind.UPTODATE);

        Comment ChangeComment(Comment original);

        List<IWritableDate> GetCommentRevisions(Comment comment);
        Comment RemoveComment(Comment comment);

        ICollection<Comment> GetMostUpToDateCommentList(Article article);
    }
}