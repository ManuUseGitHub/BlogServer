namespace Entities.EmmBlog.DataModelObjects
{
    public interface IHaveFriendshipStatus
    {
        public int? StatusId { get; set; }

        public Status Status { get; set; }

        public bool IsBlocking { get; set; }
    }
}