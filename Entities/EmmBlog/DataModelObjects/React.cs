using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("React")]
    public class React : IHaveArticle
    {
        #region Keys

        [Key]
        [Column(Order = 1)]
        public Guid? AccountId { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("TypeId")]
        public int? TypeId { get; set; }

        [Column("ReactionType")]
        public ReactionType Reaction { get; set; }

        [Key]
        [Column(Order = 3)]
        public string ItemId { get; set; }

        #endregion Keys

        #region ForeingKeys

        [ForeignKey("slug")]
        public string Slug { get; set; }

        [ForeignKey("VDepth")]
        public int? VDepth { get; set; }

        [ForeignKey("blogId")]
        public Guid? BlogId { get; set; }

        [ForeignKey("CommentId")]
        public Guid? CommentId { get; set; }
        [ForeignKey("RDepth")]
        public int? RDepth { get; set; }

        #endregion ForeingKeys

        #region Entities

        public Article Article { get; set; }
        public Comment Comment { get; set; }
        public Account Account { get; set; }

        #endregion Entities
    }
}