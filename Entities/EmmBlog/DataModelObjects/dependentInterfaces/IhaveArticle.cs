using Entities.EmmBlog.DataModelObjects;
using System;

namespace Entities.EmmBlog.DataModelObjects.dependentInterfaces
{
    public interface IHaveArticle
    {
        public int? VDepth { get; set; }
        public string Slug { get; set; }
        public Guid? BlogId { get; set; }
        public Article Article { get; set; }
    }
}