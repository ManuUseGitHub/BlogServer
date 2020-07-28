using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("notification")]
    public class Notification : IWritableDate
    {
        [Key]
        public Guid? NotifId { get; set; }
        public string Content { get; set; }

        [ForeignKey("KindId")]
        public int? KindId { get; set; }

        public Kind Kind {get;set;}

        public Account Account { get; set; }

        [ForeignKey("AccountId")]
        public Guid? AccountId { get; set; }

        public DateTime? WriteDate { get; set; }

        public bool? Seen { get; set; }
    }
}
