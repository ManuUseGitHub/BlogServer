using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("Message")]
    public class Message : IWritableDate
    {
        [Key]
        [Column(Order = 1)]
        public long? MessageId { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("FromId")]
        public Guid? FromId { get; set; }

        public Account From { get; set; }

        [Key]
        [Column(Order = 3)]
        [ForeignKey("ToId")]
        public Guid? ToId { get; set; }

        public Account To { get; set; }

        public string Content { get; set; }
        public DateTime? WriteDate { get; set; }

        public Message Answer { get; set; }

        [ForeignKey("AnswerId")]
        public long? AnswerId { get; set; }

        [ForeignKey("AnswerToId")]
        public Guid? AnswerToId { get; set; }

        [ForeignKey("AnswerFromId")]
        public Guid? AnswerFromId { get; set; }

        public State State { get; set; }

        [ForeignKey("StateId")]
        public int? StateId { get; set; }
    }
}