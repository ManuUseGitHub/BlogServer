using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("ReactionType")]
    public class ReactionType
    {
        public enum SocialReaction {
            Like = 1,
            Love = 2,
            Haha = 3,
            Surprised = 4,
            Sad = 5,
            Angry = 6,
            Confused = 7,
        }

        [Key]
        public int? TypeId { get; set; }

        public string Label { get; set; }
    }
}