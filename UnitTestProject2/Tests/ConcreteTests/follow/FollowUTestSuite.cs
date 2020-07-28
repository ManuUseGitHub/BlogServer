using Entities.EmmBlog.DataModelObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using static Repository.Exceptions.AccountsExceptions;

namespace UnitTestProject.Tests.ConcreteTests
{
    public partial class FollowUTest : UnitTest
    {
        [ClassInitialize]
        public static void FollowInit(TestContext context)
        {
            foreach (string mail in new string[] {
                "j.doe@gmail.com",
                "j.fehtou@gmail.com",
                "j.bonvin@gmail.com",
                "a.bonaventure@gmail.com",
                "j.gnack@gmail.com"
            }
            )
            {
                Account johndoe = Wrapper.Account
                .GetAccountByMailAddress(mail);

                Wrapper.Follow.CreateAFollow(
                NullNormalized<Follow>(f =>
                {
                    f.Following = johndoe;
                    f.Followed = JeanLucEmmanuel;
                })
            );
            }
        }

        [TestMethod]
        public void AnAccountCanFollowAnother()
        {
            Follow friendShip = MakeFollowing("jean.luc.e.verhan@gmail.com", "manmdyalw@gmail.com");
            
            Wrapper.Follow.CreateAFollow(friendShip);

            Follow first = Wrapper.Follow.GetFollow(new Follow()
            {
                FollowingId = Manu.Id,
                FollowedId = JeanLucEmmanuel.Id
            });

            Assert.IsNotNull(first);
        }


        [TestMethod]
        public void ANotAcceptedYetFriendAccountCanOnlySeePublicBlogAricles()
        {
            Account acc = JeanLucEmmanuel;
            Account viewer = Wrapper.Account
                .GetAccountByMailAddress("j.doe@gmail.com");

            Wrapper.Article.CreateArticle(new Article()
            {
                Title = "For friends only",
                Content = "What an interressant content !!!",
                Blog = Wrapper.Blog.GetBlogOfAccount(acc.Id),
                VisibilityId = 2
            });

            Blog blog = Wrapper.Blog.GetBlogOfAccount(acc.Id);
            Blog blogResult = Wrapper.Blog.GetEndBlogOfAccount(acc.Id, viewer.Id);

            Assert.AreNotEqual(blog.Articles.Count, blogResult.Articles.Count);
        }

        [TestMethod]
        public void AFollowingAccountCreatesANotification()
        {
            string followed = "manmdyalw@gmail.com";
            string following = "j.fehtou@gmail.com";

            Account acc = Wrapper
                    .Account
                    .GetAccountByMailAddress(followed);

            int c1 = Wrapper.Notificaton.GetNotoficationsOf(acc).Count;

            Follow friendship = MakeFollowing(followed, following);
            Wrapper.Follow.CreateAFollow(friendship);


            Wrapper.Follow.ClaimAFriendShip(friendship);

            int c2 = Wrapper.Notificaton.GetNotoficationsOf(acc).Count;

            Assert.AreNotEqual(c1, c2);
        }

        [TestMethod]
        public void AnAcceptedAccountGetsANotification()
        {
            string followed = "manmdyalw@gmail.com";
            string following = "a.bonaventure@gmail.com";

            Account acc = Wrapper
                    .Account
                    .GetAccountByMailAddress(following);

            int c1 = Wrapper.Notificaton.GetNotoficationsOf(acc).Count;

            Follow friendship = MakeFollowing(followed, following);

            Wrapper.Follow.CreateAFollow(friendship);
            Wrapper.Follow.ClaimAFriendShip(friendship);
            Wrapper.Follow.AcceptFriend(friendship);

            int c2 = Wrapper.Notificaton.GetNotoficationsOf(acc).Count;

            Assert.AreNotEqual(c1, c2);
        }

        [TestMethod]
        public void AnyAccountCanBeBlocked()
        {
            Account acc = Wrapper
                    .Account
                    .SearchByFullName("Jacko Gnack").FirstOrDefault();

            Follow blocking = NullNormalized<Follow>(f =>
            {
                f.Following = acc;
                f.Followed = Manu;
            });

            Wrapper.Follow.MakeABlock(blocking);

            Follow follow = Wrapper.Follow.GetBlockedBy(Manu)
                .Where(b => b.Following.FirstName.Equals("Jacko"))
                .FirstOrDefault();

            Assert.IsNotNull(follow);
        }

        [TestMethod]
        public void UnblockedAccountsAreListedAgain()
        {
            List<Follow> followings = new List<Follow>();

            int cpt = 0;
            foreach(Follow blocking in new Follow[] {
                MakeFollowingToAccount(Manu,"Mick Ado"), 
                MakeFollowingToAccount(Manu, "Lamas Retho") 
            })
            {
                if (cpt++ == 0)
                {
                    Wrapper.Follow.CreateAFollow(blocking);
                }

                Follow blocked = Wrapper.Follow.MakeABlock(blocking);
                Follow unblocked = Wrapper.Follow.RemoveABlock(blocking);


                followings.Add( unblocked );
            }

            // if unblocked, potentialy friend is still considered following
            Assert.IsNotNull(followings.ElementAt(0));
            
            // if unblocked, a non friend stay unlisted
            Assert.IsNull(followings.ElementAt(1));
        }

        [TestMethod,ExpectedException(typeof(NotFoundAccountException))]
        public void BlockedAccountsCannotBeFoundInSearch()
        {
            Account acc = Wrapper
                   .Account
                   .SearchByFullName("Cezar Salad").FirstOrDefault();

            Follow blocking = NullNormalized<Follow>(f =>
            {
                f.Following = acc;
                f.Followed = Manu;
            });

            Wrapper.Follow.MakeABlock(blocking);

            Account found = Wrapper.Account
                .SearchByFullName("Cezar Salad",Manu.Id)
                .FirstOrDefault();

            Assert.IsNull(found);
        }

        [TestMethod]
        public void AFriendAcceptationCreatesTheFrienRelationOfBothSide()
        {
            Follow requested = MakeFollowingFullNames("Musashi Kojiro", "Rose Beaf");
            Follow reciproquee = MakeFollowingFullNames("Rose Beaf", "Musashi Kojiro");

            Wrapper.Follow.ClaimAFriendShip(requested);
            Wrapper.Follow.AcceptFriend(requested);

            Follow created = Wrapper.Follow.GetFollow(reciproquee);

            Assert.IsNotNull(created);
        }

        [TestMethod]
        public void AFriendRemovalRemovesBothRelation()
        {
            Follow requested = MakeFollowingFullNames("Musashi Kojiro", "Selfe Stiim");
            Follow reciproquee = MakeFollowingFullNames("Selfe Stiim", "Musashi Kojiro");

            Wrapper.Follow.ClaimAFriendShip(requested);
            Wrapper.Follow.AcceptFriend(requested);
            Wrapper.Follow.RemoveFriendship(requested);

            Follow created = Wrapper.Follow.GetFollow(reciproquee);

            Assert.IsNull(created);
        }

        [TestMethod]
        public void ABlockedAccountCannotBeFound()
        {
            Account acc = Wrapper
                    .Account
                    .SearchByFullName("Tom Ato").FirstOrDefault();

            Follow blocking = NullNormalized<Follow>(f =>
            {
                f.Following = acc;
                f.Followed = Manu;
            });

            Wrapper.Follow.MakeABlock(blocking);

            ICollection<Follow> authorized = Wrapper.Account
                .GetAccountById(blocking.Followed.Id)
                .Followers;

            ICollection<Follow> nonAuthorized = Wrapper.Account
                .GetEndAccountById(blocking.Followed.Id)
                .Followers;

            // not found in friend list (filtered)
            Assert.IsNull(nonAuthorized
                    .Where(b => b.Following.FirstName.Equals("Tom"))
                    .FirstOrDefault()
                );

            // found in friend list (default)
            Assert.IsNotNull(authorized
                    .Where(b => b.Following.FirstName.Equals("Tom"))
                    .FirstOrDefault()
                );
        }

        [TestMethod]
        public void ABlockingAccountCanFindAListOfBlockedAccounts()
        {
            Account acc = Wrapper
                    .Account
                    .SearchByFullName("John Doe").FirstOrDefault();

            Follow blocking = NullNormalized<Follow>(f =>
            {
                f.Following = acc;
                f.Followed = Manu;
            });

            int c1 = Wrapper.Follow.GetBlockedBy(blocking.Followed).Count;

            Wrapper.Follow.MakeABlock(blocking);

            int c2 = Wrapper.Follow.GetBlockedBy(blocking.Followed).Count;

            Assert.AreNotEqual(c1, c2);
        }
    }
}