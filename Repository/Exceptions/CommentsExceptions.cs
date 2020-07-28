using Entities.EmmBlog.DataModelObjects;
using System;
using System.Text;

namespace Repository.Exceptions
{
    public class CommentsExceptions : EmmBlogException<Comment>
    {
        public class NoCommentFoundException : NullReferenceException
        {
            public NoCommentFoundException(string message) : base(message)
            { }

            public NoCommentFoundException(Comment comment)
                : base(ExpliciteNotFoundMessage(comment,
                    new object[] { comment.Id,comment.RDepth }
                )
            )
            { }
        }

        internal class NoAnsweredCommentCanBeDeletedException : InvalidOperationException
        {
            public static string ExcpliciteCannotDelete(Comment comment)
            {
                string identifying = $"[key:{comment.Id}|{comment.RDepth}]";
                string answering = $"[key:{comment.AnswerId}|{comment.Answer.AnswerRevision}]";

                return new StringBuilder()
                    .Append($"\r\nThe comment given by {identifying} has another Comment ")
                    .Append($"that is refering to it ({answering})")
                    .Append("\r\nRemove the answering then proceed or set the visibility of the desired comment to removed ")
                    .Append("by setting the visibilityId to 99").ToString();
            }
            public NoAnsweredCommentCanBeDeletedException(string message) : base(message)
            { }

            public NoAnsweredCommentCanBeDeletedException(Comment comment)
                : base(ExcpliciteCannotDelete(comment))
            { }
        }
    }
}