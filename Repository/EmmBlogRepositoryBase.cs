using Contracts;
using Entities;
using static Entities.EmmBlog.DataModelObjects.Importance;
using static Entities.EmmBlog.DataModelObjects.ReactionType;
using static Entities.EmmBlog.DataModelObjects.Status;
using static Entities.EmmBlog.DataModelObjects.Visibility;
using static Entities.EmmBlog.DataModelObjects.Kind;
using Entities.EmmBlog.DataModelObjects;
using static Entities.EmmBlog.DataModelObjects.State;

namespace Repository
{
    public class EmmBlogRepositoryBase<T> : RepositoryBase<T> where T : class, new()
    {
        public EmmBlogRepositoryBase(
            IRepositoryWrapper wrapper, RepositoryContext repositoryContext
        ) : base(wrapper, repositoryContext)
        { }

        #region TYPES

        // Socials
        public static int LIKE = (int)SocialReaction.Like;
        public static int LOVE = (int)SocialReaction.Love;
        public static int HAHA = (int)SocialReaction.Haha;
        public static int ANGRY = (int)SocialReaction.Angry;
        public static int CONFUSED = (int)SocialReaction.Confused;
        public static int SURPRISED = (int)SocialReaction.Surprised;
        public static int SAD = (int)SocialReaction.Sad;

        // Visibilities

        public static int PUBLIC = (int)ContentVisibility.Public;
        public static int FRIENDS = (int)ContentVisibility.Friends;
        public static int PERSONAL = (int)ContentVisibility.Personal;
        public static int VHIDDEN = (int)ContentVisibility.Hidden;
        public static int REMOVED = (int)ContentVisibility.Removed;


        // Status

        public const int F_FRIEND = (int)FollowStatus.Friend;
        public const int F_PENDINGA = (int)FollowStatus.PendingAlpha;
        public const int F_PENDINGB = (int)FollowStatus.PendingBeta;
        public const int F_FOLLOW = (int)FollowStatus.Following;
        public const int F_BLOCKED = (int)FollowStatus.Blocked;

        // State

        public const int M_SENDING = (int)MessageState.Sending;
        public const int M_SENT = (int)MessageState.Sent;
        public const int M_UNSENT = (int)MessageState.Unsent;
        public const int M_REMOVED = (int)MessageState.Removed;

        // Importance

        public const int DEFAULT = (int)FollowImportance.Default;
        public const int FIRST = (int)FollowImportance.First;
        public const int HIDDEN = (int)FollowImportance.Hidden;

        public const int SYSTEM = (int)NotificationKind.System;
        public const int BLOG = (int)NotificationKind.BLog;
        public const int SOCIAL = (int)NotificationKind.Social;
        public const int MESSENGER = (int)NotificationKind.Messenger;


        #endregion TYPES
    }
}