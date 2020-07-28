using System;
using Entities.EmmBlog.DataModelObjects;

namespace Entities.EmmBlog.DataTransferObjects
{
    public class SubscribeDTO
    {
        public Guid AccountId { get; set; }
        public Account Account { get; set; }
    }
}
