using Repository.Exceptions;
using System;
using static Entities.EmmBlog.DataModelObjects.Status;
using static Repository.Exceptions.FollowStatusExceptions;

namespace Entities.EmmBlog.DataModelObjects
{
    public static class StatusStateMachine<HasStatus> 
        where HasStatus : IHaveFriendshipStatus
    {
        public static void BlockAccount(HasStatus obj)
        {
            ThrowWrongStateIf<WrongBlockingException>(obj,
                "!".Equals(GetState(obj))
            );
        }

        public static void AllowAccount(HasStatus obj)
        {
            ThrowWrongStateIf<WrongAllowingException>(obj,
                !"!".Equals(GetState(obj))
            );
        }

        public static void ClaimFsRequest(HasStatus obj)
        {
            string state = GetState(obj);

            ThrowWrongStateIf<WrongClaimingException>(obj,
                !("δ".Equals(state) || "0".Equals(state))
            );
        }

        public static void CancelFsRequest(HasStatus obj)
        {
            string state = GetState(obj);

            ThrowWrongStateIf<WrongCancellationRequest>(obj,
                !("1α".Equals(state) || "1β".Equals(state))
            );
        }

        public static void RemoveFriendship(HasStatus obj)
        {
            ThrowWrongStateIf<WrongFriendshipRemoval>(obj,
                !"2".Equals(GetState(obj))
            );
        }

        public static void CreateFriendship(HasStatus obj)
        {
            ThrowWrongStateIf<WrongFriendshipCreation>(obj,
                !"δ".Equals(GetState(obj))
            );
        }

        public static void FollowAccount(HasStatus obj)
        {
            ThrowWrongStateIf<WrongFollowException>(obj,
                !"δ".Equals(GetState(obj))
            );
        }

        public static void StopFollowingAccount(HasStatus obj)
        {
            ThrowWrongStateIf<WrongUnfollowingException>(obj,
                !"0".Equals(GetState(obj))
            );
        }

        public static void AcceptFriendShip(HasStatus obj)
        {
            string state = GetState(obj);

            ThrowWrongStateIf<WrongFriendshipAcceptation>(obj,
                !("1α".Equals(state) || "1β".Equals(state))
            );
        }

        public static void ThrowWrongStateIf<T>(HasStatus obj, bool condition)
            where T : Exception
        {
            if (condition)
            {
                DbEx.Instance.Throw<T>(obj);
            }
        }

        public static string GetState(HasStatus obj)
        {
            if (obj == null) return "δ"; // Den (δ)εν <=> not (for not created)
            if (obj.IsBlocking) return "!";
            if (obj.StatusId == (int)FollowStatus.Following) return "0";
            if (obj.StatusId == (int)FollowStatus.PendingAlpha) return "1α";
            if (obj.StatusId == (int)FollowStatus.PendingBeta) return "1β";
            if (obj.StatusId == (int)FollowStatus.Friend) return "2";

            throw new InvalidOperationException("State unknown");
        }
    }
}