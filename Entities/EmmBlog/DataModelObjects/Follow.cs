using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    public class Follow : IHaveFriendshipStatus
    {
        [Key]
        [Column(Order = 1)]
        [ForeignKey("FollowingId")]
        public Guid? FollowingId { get; set; }

        public Account Following { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("FollowedId")]
        public Guid? FollowedId { get; set; }

        public Account Followed { get; set; }

        public bool IsBlocking { get; set; }
        public DateTime? FollowingDate { get; set; }

        [ForeignKey("StatusId")]
        public int? StatusId { get; set; }

        public Status Status { get; set; }

        [ForeignKey("ImportanceId")]
        public int? ImportanceId { get; set; }

        public Importance Importance { get; set; }
    }
}