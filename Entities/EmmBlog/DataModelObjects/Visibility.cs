namespace Entities.EmmBlog.DataModelObjects
{
    public class Visibility
    {
        public enum ContentVisibility
        {
            Public = 1,
            Friends = 2,
            Personal = 3,
            Hidden = 4,

            // ...

            Removed = 99
        }

        public int? VisibilityId { get; set; }

        public string label { get; set; }
    }
}