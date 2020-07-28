using System;
using System.Collections.Generic;
using System.Text;
using Entities.EmmBlog.DataModelObjects;

namespace Entities.EmmBlog.DataTransferObjects
{
    public class AccountDTO
    {
        public Guid Id { get; set; }

        public String FirstName { get; set; }
        public String LastName { get; set; }
        
        public DateTime DateOfBirth { get; set; }
        public String Avatar { get; set; }
        public String UserName { get; set; }
        public Boolean wantsNewsLetter { get; set; }

        public virtual ICollection<Subscribe> Subscribe { get; set; }
    }
}
