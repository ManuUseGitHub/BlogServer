using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;
using static Entities.EmmBlog.DataTransferObjects.Out.Reduced.BlogWithArticlesDTO;

namespace Entities.EmmBlog.DataTransferObjects.Out.Reduced
{
    public class ArticlesOfBlogDTO : WithReactionsAndCommentsDTO
    {

        private string slug;
        public string Slug { get { return "hello"; } set { slug = value; } }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime WriteDate { get; set; }

        public Blog Blog { private get; set; }

        public Account Account { private get { return Blog.Account; } set { Blog.Account = value; } }

        public Guid? Author { get { return Account.Id; } }
        public string AuthorName
        {
            // readonly
            get { return Account.FirstName + " " + Account.LastName; }
        }

        public string AuthorAvatar { get { return Account.Avatar; } }
        public string AuthorUserName { get { return Account.UserName; } }

        public ICollection<CommentOfArticleDTO> Comments { get; set; }

        public ICollection<ArticleRevisionDTO> Revisions { get; set; }
    }
}
