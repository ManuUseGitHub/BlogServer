using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("Share")]
    public class Share : IWritableDate , IHaveArticle
    {
        public Article Article { get; set; }
        public Blog Blog { get; set; }
        public DateTime? WriteDate { get; set; }

        [Key]
        [Column(Order = 1)]
        [ForeignKey("Slug")]
        public string Slug { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("BlogId")]
        public Guid? BlogId { get; set; }

        [Key]
        [Column(Order = 3)]
        [ForeignKey("VDepth")]
        public int? VDepth { get; set; }

        [Key]
        [Column(Order = 4)]
        [ForeignKey("SharingBlogId")]
        public Guid? SharingBlogId { get; set; }
    }
}