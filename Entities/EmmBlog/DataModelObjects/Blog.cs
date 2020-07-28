using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("blog")]
    public class Blog : IHaveAccount
    {
        public Blog()
        {
            Articles = new List<Article>();
            Shares = new List<Share>();
        }

        public new string ToString()
        {
            return $"[B:{BlogName}] [from:{Account.LastName}|{Account.FirstName}|{Account.UserName}]";
        }

        [Key]
        [Column("BlogId")]
        public Guid? Id { get; set; }

        public String BlogName { get; set; }

        public ICollection<Subscribe> Subscribe { get; set; }
        public ICollection<Article> Articles { get; set; }

        [ForeignKey("OwnerId")]
        [Column("OwnerId")]
        public Guid? AccountId { get; set; }

        public Account Account { get; set; }
        public ICollection<Comment> Comments { get; set; }

        public Visibility Visibility { get; set; }
        public int? VisibilityId { get; set; }

        public ICollection<Share> Shares { get; set; }
    }
}