using Entities.EmmBlog.DataModelObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.EmmBlog.DataTransferObjects.Out.Reduced
{
    public class ArticleRevisionDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime WriteDate { get; set; }
    }
}
