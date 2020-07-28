using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    public class Subscribe : IWritableDate, IHaveAccount
    {
        [Key]
        [Column(Order = 1)]
        public Guid? BlogId { get; set; }

        public Blog Blog { get; set; }

        [Key]
        [Column(Order = 2)]
        public Guid? AccountId { get; set; }

        public Account Account { get; set; }

        public DateTime? WriteDate { get; set; }
    }
}