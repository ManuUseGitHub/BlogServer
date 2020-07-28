using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("credential")]
    public class Credential : IHaveAccount
    {
        [Key]
        [Required(ErrorMessage = "Mail address is required")]
        [StringLength(50, ErrorMessage = "Mail address can't be longer than 50 characters")]
        public String MailAddress { get; set; }

        public DateTime RegistrationDate { get; set; }

        [Required(ErrorMessage = "The password is required")]
        public String PassWord { get; set; }

        [ForeignKey("AccountId")]
        public Guid? AccountId { get; set; }

        public Account Account { get; set; }
    }
}