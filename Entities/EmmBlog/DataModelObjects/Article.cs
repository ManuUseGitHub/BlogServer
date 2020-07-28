using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("Article")]
    public class Article : IHaveReactions
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime? WriteDate { get; set; }

        [Key]
        [Column(Order = 1)]
        public string Slug { get; set; }

        [Key]
        [Column(Order = 2)]
        public int? VDepth { get; set; }

        [Key]
        [Column(Order = 3)]
        [ForeignKey("blogId")]
        public Guid? BlogId { get; set; }

        public int? VisibilityId { get; set; }

        public Visibility Visibility { get; set; }

        public ICollection<Comment> Comments { get; set; }
        public ICollection<Share> Shares { get; set; }

        [Column("Reacts")]
        public ICollection<React> Reactions { get; set; }

        [NotMapped]
        public ICollection<ReactionCounter> ReactionCounts { get; set; }

        [NotMapped]
        public ICollection<Article> Revisions { get; set; }

        public Blog Blog { get; set; }
    }
}