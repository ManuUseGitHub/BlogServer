using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.EmmBlog.DataModelObjects
{
    [Table("Connexion")]
    public class Connexion : IHaveAccount, IWritableDate
    {
        [Key]
        [Column("ConnexionId", Order = 1)]
        public Guid Id { get; set; }

        [Key]
        [Column(Order = 2)]
        [ForeignKey("AccountId")]
        public Guid? AccountId { get; set; }

        public Account Account { get; set; }

        public DateTime? WriteDate { get; set; }

        public string Platform { get; set; }

        public Boolean Valid { get; set; }

        public string Geographic { get; set; }

        public float Longitude { get; set; }

        public float Latitude { get; set; }
    }
}