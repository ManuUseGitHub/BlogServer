using System;

namespace Entities.EmmBlog.DataModelObjects.dependentInterfaces
{
    public interface IWritableDate
    {
        public DateTime? WriteDate { get; set; }
    }
}
