using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("Comment")]
    public class Comment : IWritableDate, IHaveReactions, IHaveArticle
    {
        #region Payload

        public string Content { get; set; }
        public DateTime? WriteDate { get; set; }

        public new string ToString()
        {
            string answering = Article == null ? Answer.ToString() : Article.ToString();
            return $"{Content}\r\nFrom:{Account}\r\nOn:{WriteDate}\r\nAnswering:{answering}";
        }

        #endregion Payload

        #region Foreign Entities

        public Account Account { get; set; }
        public Comment Answer { get; set; }
        public Visibility Visibility { get; set; }
        public ICollection<Comment> Answers { get; set; }

        [NotMapped]
        public ICollection<ReactionCounter> ReactionCounts { get; set; }

        [Column("Reacts")]
        public ICollection<React> Reactions { get; set; }

        #endregion Foreign Entities

        #region Key and foreign keys

        [Key]
        [Column("CommentId", Order = 1)]
        public Guid? Id { get; set; }

        [Key]
        [Column("RDepth", Order = 2)]
        public int? RDepth { get; set; }

        #endregion Key and foreign keys

        #region ForeignKey

        [ForeignKey("AnswerId")]
        public Guid? AnswerId { get; set; }

        [ForeignKey("AnswerRevision")]
        public int? AnswerRevision { get; set; }

        [ForeignKey("AccountId")]
        public Guid? AccountId { get; set; }

        [ForeignKey("VisibilityId ")]
        public int? VisibilityId { get; set; }

        [ForeignKey("slug")]
        public string Slug { get; set; }

        public Article Article { get; set; }

        [ForeignKey("VDepth")]
        public int? VDepth { get; set; }

        [ForeignKey("BlogId")]
        public Guid? BlogId { get; set; }

        #endregion ForeignKey
    }
}