using Entities.EmmBlog.DataModelObjects;
using System;

namespace Repository.Exceptions
{
    public partial class FollowStatusExceptions : EmmBlogException<IHaveFriendshipStatus>
    {
        public class WrongBlockingException : WrongStatusException
        {
            public WrongBlockingException(Follow friendshipStatus)
                : base(friendshipStatus, "!")
            { }

            public WrongBlockingException(string message)
                : base(message)
            { }
        }

        public class WrongAllowingException : WrongStatusException
        {
            public WrongAllowingException(IHaveFriendshipStatus friendshipStatus)
                : base(friendshipStatus, "{δ, 0, 1α, 1β ,2}")
            { }

            public WrongAllowingException(string message)
                : base(message)
            { }
        }
    }
}