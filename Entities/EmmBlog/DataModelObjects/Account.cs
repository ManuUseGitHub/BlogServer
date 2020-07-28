using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("account")]
    public class Account
    {
        public Account()
        {
            UserName = "UNKNOWN";
        }

        public new string ToString()
        {
            return $"{LastName.ToUpper()} {FirstName} {UserName}";
        }

        [Column("AccountId")]
        public Guid? Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string Avatar { get; set; }
        public string UserName { get; set; }
        public bool wantsNewsLetter { get; set; }

        public virtual ICollection<Subscribe> Subscribe { get; set; }
        public Credential Credential { get; set; }

        public Visibility Visibility { get; set; }
        public int VisibilityId { get; set; }
        public ICollection<Follow> Followings { get; set; }
        public ICollection<Follow> Followers { get; set; }

        public ICollection<Notification> Notifications { get; set; }
    }
}