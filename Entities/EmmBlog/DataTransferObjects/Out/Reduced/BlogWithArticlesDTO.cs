using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;

namespace Entities.EmmBlog.DataTransferObjects.Out.Reduced
{
    public class BlogWithArticlesDTO
    {
        public Guid Id { get; set; }
        public string BlogName { get; set; }

        public ICollection<ArticlesOfBlogDTO> Articles { get; set; }

        public class CommentOfArticleDTO : WithReactionsAndCommentsDTO
        {
            public Guid CommentId { get; set; }

            public string content { get; set; }

            public DateTime writeDate { get; set; }

            public Guid AccountId { get; set; }

            public Account Account { private get; set; }

            public string Avatar { get { return Account.Avatar; } }

            public string User
            {
                // readonly
                get { return Account.FirstName + " " + Account.LastName; }
            }

            public AnswereOfCommentDTO Answer { private get; set; }

            public ICollection<AnswereOfCommentDTO> Answers { get; set; }
        }

        public class AnswereOfCommentDTO : CommentOfArticleDTO
        {
            public new ICollection<CommentOfArticleDTO> Answers { private get; set; }

        }
    }
}
