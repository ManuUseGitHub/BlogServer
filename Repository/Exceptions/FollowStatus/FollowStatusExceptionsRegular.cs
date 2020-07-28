using Entities.EmmBlog.DataModelObjects;
using System;

namespace Repository.Exceptions
{
    public partial class FollowStatusExceptions : EmmBlogException<IHaveFriendshipStatus>
    {
        public class WrongCancellationRequest : WrongStatusException
        {
            public WrongCancellationRequest(IHaveFriendshipStatus friendshipStatus)
                : base(friendshipStatus, "δ")
            { }

            public WrongCancellationRequest(string message) : base(message)
            { }
        }

        public class WrongFriendshipRemoval : WrongStatusException
        {
            public WrongFriendshipRemoval(IHaveFriendshipStatus friendshipStatus)
                : base(friendshipStatus, "0")
            { }

            public WrongFriendshipRemoval(string message) : base(message)
            { }
        }

        public class WrongUnfollowingException : WrongStatusException
        {
            public WrongUnfollowingException(IHaveFriendshipStatus friendshipStatus)
                : base(friendshipStatus, "δ")
            { }

            public WrongUnfollowingException(string message) : base(message)
            { }
        }
        public class WrongFriendshipAcceptation : WrongStatusException
        {
            public WrongFriendshipAcceptation(IHaveFriendshipStatus friendshipStatus)
                : base(friendshipStatus, "2")
            { }

            public WrongFriendshipAcceptation(string message) : base(message)
            { }
        }
    }
}