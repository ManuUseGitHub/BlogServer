using Entities.EmmBlog.DataModelObjects;
using System;

namespace Repository.Exceptions
{
    public partial class FollowStatusExceptions : EmmBlogException<IHaveFriendshipStatus>
    {
        #region Base class

        public abstract class WrongStatusException : InvalidOperationException
        {
            public WrongStatusException(
                IHaveFriendshipStatus friendshipStatus, string finalityState)

                : base($"{GetExpliciteMessage(friendshipStatus, finalityState)}"
                )
            { }

            public WrongStatusException(string message)
                : base($"\r\nWrong state :\r\n{message}\r\n")
            { }

            private static string GetExpliciteMessage(
                IHaveFriendshipStatus friendshipStatus, 
                string finalityState = "?"
            )
            {
                string startingState = StatusStateMachine<IHaveFriendshipStatus>
                    .GetState(friendshipStatus);

                return $"[{startingState} -> {finalityState}]";
            }
        }

        #endregion Base class
    }
}