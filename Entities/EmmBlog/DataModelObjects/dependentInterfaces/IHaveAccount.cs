using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EmmBlog.DataModelObjects.dependentInterfaces
{
    public interface IHaveAccount
    {
        public Account Account { get; set; }
    }
}
