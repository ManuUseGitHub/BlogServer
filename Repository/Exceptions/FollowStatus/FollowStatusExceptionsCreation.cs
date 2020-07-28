using Entities.EmmBlog.DataModelObjects;
using System;

namespace Repository.Exceptions
{
    public partial class FollowStatusExceptions : EmmBlogException<IHaveFriendshipStatus>
    {
        public class WrongClaimingException : WrongStatusException
        {
            public WrongClaimingException(IHaveFriendshipStatus friendshipStatus)
                : base(friendshipStatus, "{1α or 1β}")
            { }

            public WrongClaimingException(string message) : base (message) { }
        }


        public class WrongFriendshipCreation : WrongStatusException
        {
            public WrongFriendshipCreation(IHaveFriendshipStatus friendshipStatus)
                : base(friendshipStatus,"2")
            { }

            public WrongFriendshipCreation(string message) : base(message)
            { }
        }
        
        public class WrongFollowException : WrongStatusException
        {
            public WrongFollowException(IHaveFriendshipStatus friendshipStatus)
                : base(friendshipStatus, "0")
            { }

            public WrongFollowException(string message) : base(message)
            { }
        }
    }
}