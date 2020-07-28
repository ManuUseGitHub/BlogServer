using Entities.EmmBlog.DataModelObjects;
using System;

namespace Entities.EmmBlog.DataTransferObjects.Out.Reduced
{
    public class PureArticleDTO
    {
        public string Slug { private get; set; }

        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime WriteDate { get; set; }

        public Account Account { private get; set; }

        public string AuthorName
        {
            // readonly
            get { return Account.FirstName + " " + Account.LastName; }
        }

        public string AuthorAvatar { get { return Account.Avatar; } }
        public string AuthorUserName { get { return Account.UserName; } }
    }
}