using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EmmBlog.DataTransferObjects.Out.Reduced
{
    public class ListedBlogDTO
    {
        public ListedBlogDTO()
        {
            Articles = new List<Article>();
            Subscribe = new List<Subscribe>();
        }

        public new string ToString()
        {
            return $"[B:{BlogName}] [from:{Account.LastName}|{Account.FirstName}|{Account.UserName}]";
        }

        public Guid Id { get; set; }
        public string BlogName { get; set; }

        public ICollection<Article> Articles { private get; set; }

        public int ArticleCount { get { return Articles.Count; } }

        public ICollection<Subscribe> Subscribe { private get; set; }
        public int Subscribers { get { return Subscribe.Count; } }

        public Account Account { private get; set; }

        public String Owner { get { return $"{Account.FirstName} {Account.LastName}"; }}

        public Visibility Visibility { private get; set; }

        public string Visible { get { return Visibility.label; } }

    }
}
