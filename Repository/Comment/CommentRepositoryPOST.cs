using Entities.EmmBlog.DataModelObjects;
using Utilities;
using System;

namespace Repository
{
    public partial class CommentRepository : EmmBlogRepositoryBase<Comment>, ICommentRepository
    {
        public Comment CommentArticle(Comment comment)
        {
            Comment merged = new Comment()
            {
                WriteDate = DateTime.Now,
                Id = Guid.NewGuid(),

                BlogId = comment.Article.BlogId,
                Slug = comment.Article.Slug,
                VDepth = comment.Article.VDepth,
                VisibilityId = PUBLIC,
                RDepth = 0
            };

            Reflector.Merge(merged, comment);

            Create(merged);
            TrySave();

            Wrapper.Notificaton.CreateCommented(merged.Article.Blog.Account, merged.Account);

            Comment found = GetComment(merged);
            return found;
        }

        public Comment AnswerComment(Comment comment)
        {
            Comment merged = new Comment()
            {
                WriteDate = DateTime.Now,
                Id = Guid.NewGuid(),
                RDepth = 0,
                VisibilityId = PUBLIC,
                AnswerId = comment.Answer.Id,
                AnswerRevision = comment.Answer.RDepth,
            };
            Utilities.Reflector.Merge(merged, comment);

            Create(merged);
            TrySave();

            Comment found = GetComment(merged);
            return found;
        }
    }
}