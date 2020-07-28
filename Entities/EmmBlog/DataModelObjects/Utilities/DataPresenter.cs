using Entities.EmmBlog.DataModelObjects.dependentInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entities.EmmBlog.DataModelObjects.Utilities
{
    public class DataPresenter
    {
        public static IEnumerable<W> GetSortedByWriteDate<W>(ICollection<W> list) where W : IWritableDate
        {
            return list
                .OrderBy(e => ((DateTime)e.WriteDate).TimeOfDay)
                .ThenBy(e => ((DateTime)e.WriteDate).Date)
                .ThenBy(e => ((DateTime)e.WriteDate).Year)
                .ThenBy(e => ((DateTime)e.WriteDate).Hour)
                .ThenBy(e => ((DateTime)e.WriteDate).Minute)
                .ThenBy(e => ((DateTime)e.WriteDate).Second);
        }
    }
}
