using Entities.EmmBlog.DataModelObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using static Repository.Exceptions.FollowStatusExceptions;

namespace UnitTestProject.Tests.ConcreteTests
{
    [TestClass]
    public class FollowStatusUTest: UnitTest
    {
        private delegate void InvokeOnFollow(Follow link);

        [TestMethod, ExpectedException(typeof(WrongBlockingException))]
        public void ABlockingAccountCannotBlockAnAlreadyBlockedAccount()
        {

            Account acc = Wrapper
                    .Account
                    .SearchByFullName("Mucat Oeth").FirstOrDefault();

            Follow blocking = NullNormalized<Follow>(f =>
            {
                f.Following = acc;
                f.Followed = Manu;
            });

            Wrapper.Follow.MakeABlock(blocking);
            Wrapper.Follow.MakeABlock(blocking);
        }

        [TestMethod, ExpectedException(typeof(WrongAllowingException))]
        public void ANotBlockedgAccountCannotBeUnblocked()
        {
            Account acc = Wrapper
                    .Account
                    .SearchByFullName("Selfe Stiim").FirstOrDefault();

            Follow blocking = NullNormalized<Follow>(f =>
            {
                f.Following = acc;
                f.Followed = Manu;
            });

            Wrapper.Follow.RemoveABlock(blocking);
        }

        [TestMethod]
        public void OnlyANotExistingFriendshipCanCreateAClaimRequest()
        {
            // no friendship (not existent)
            Follow friendship = MakeFollowingToAccount(
                "Mucat Oeth", JeanLucEmmanuel
            );

            Wrapper.Follow.ClaimAFriendShip(friendship);


            Follow[] friendships = new Follow[] {
                // already claiming
                friendship,
            };

            // verify every updated relationships
            VerifyFriendShipStateChanges<WrongClaimingException>(
                friendships,
                Wrapper.Follow.ClaimAFriendShip
            );
        }

        [TestMethod]
        public void OnlyAFollowingCanClaimAFriendshipRequest()
        {
            // with a follow pass
            Follow friendship = MakeFollowingToAccount(
                "Lucilia Morning-Stark", JeanLucEmmanuel
            );
            
            Wrapper.Follow.CreateAFollow(friendship);
            Wrapper.Follow.ClaimAFriendShip(friendship);

            Follow[] friendships = new Follow[] {
                MakeFollowingFullNames("Lucilia Morning-Stark", "Selfe Stiim"),
                MakeFollowingFullNames("Mucat Oeth", "Selfe Stiim"),

                // already claiming
                friendship,
            };

            // blocking relation
            Wrapper.Follow.MakeABlock(friendships[0]);

            // Already friends
            Wrapper.Follow.ClaimAFriendShip(friendships[1]);
            Wrapper.Follow.AcceptFriend(friendships[1]);

            // verify every updated relationships
            VerifyFriendShipStateChanges<WrongClaimingException>(
                friendships, 
                Wrapper.Follow.ClaimAFriendShip
            );
        }

        [TestMethod]
        public void OnlyAPendingAlphaRequestOfFriendshipCanBeCanceled()
        {
            // with a follow pass
            Follow friendship = MakeFollowingToAccount(
                "Lucilia Morning-Stark", Manu
            );

            Wrapper.Follow.ClaimAFriendShip(friendship);
            Wrapper.Follow.CancelFriendShipRequest(friendship);
        }

        [TestMethod]
        public void OnlyAPendingBetaRequestOfFriendshipCanBeCanceled()
        {
            // with a follow pass
            Follow friendship = MakeFollowingFullNames(
                "Lucilia Morning-Stark", "Tom Ato"
            );

            Wrapper.Follow.CreateAFollow(friendship);
            Wrapper.Follow.ClaimAFriendShip(friendship);
            Wrapper.Follow.CancelFriendShipRequest(friendship);

            Follow[] friendships = new Follow[] {

                // already canceled
                friendship,

                MakeFollowingFullNames("Tom Ato","Jules Gustavo")
            };

            // blocking relation
            Wrapper.Follow.MakeABlock(friendships[0]);

            // Already friends
            Wrapper.Follow.ClaimAFriendShip(friendships[1]);
            Wrapper.Follow.AcceptFriend(friendships[1]);

            // verify every updated relationships
            VerifyFriendShipStateChanges<WrongCancellationRequest>(
                friendships,
                Wrapper.Follow.CancelFriendShipRequest
            );
        }

        [TestMethod]
        public void OnlyAPendingAlphaRequestOfFriendshipCanBeAccepted()
        {
            // with a follow pass
            Follow friendship = MakeFollowingFullNames(
                "Crishian Doe", "Tom Ato"
            );

            // no follow first before claiming
            Wrapper.Follow.ClaimAFriendShip(friendship);
            Wrapper.Follow.AcceptFriend(friendship);
        }

        [TestMethod]
        public void OnlyAPendingBetaRequestOfFriendshipCanBeAccepted()
        {
            // with a follow pass
            Follow friendship = MakeFollowingFullNames(
                "Jaydu Bon-vin", "Tom Ato"
            );

            // follow first before claiming
            Wrapper.Follow.CreateAFollow(friendship);

            Wrapper.Follow.ClaimAFriendShip(friendship);
            Wrapper.Follow.AcceptFriend(friendship);

            Follow[] friendships = new Follow[] {

                // already accepted
                friendship,

                MakeFollowingFullNames("Tom Ato","Rogue Rogue")
            };

            // blocking relation
            Wrapper.Follow.MakeABlock(friendships[0]);

            // Already friends
            Wrapper.Follow.ClaimAFriendShip(friendships[1]);
            Wrapper.Follow.AcceptFriend(friendships[1]);

            // verify every updated relationships
            VerifyFriendShipStateChanges<WrongFriendshipAcceptation>(
                friendships,
                Wrapper.Follow.AcceptFriend
            );
        }

        [TestMethod]
        public void OnlyAnAlreadyFriendCanAskAFriendshipRemoval()
        {
            // with a follow pass
            Follow friendship = MakeFollowingFullNames(
                "Garcia Gomez", "Jaydu Bon-vin"
            );

            Wrapper.Follow.CreateAFollow(friendship);
            Wrapper.Follow.ClaimAFriendShip(friendship);
            Wrapper.Follow.AcceptFriend(friendship);
            Wrapper.Follow.RemoveFriendship(friendship);

            Follow[] friendships = new Follow[] {

                // already removed
                friendship,

                MakeFollowingFullNames("Garcia Gomez","Rogue Rogue"),

                MakeFollowingFullNames("Garcia Gomez","Tom Ato")
            };

            // blocking
            Wrapper.Follow.MakeABlock(friendships[0]);

            // claiming alpha
            Wrapper.Follow.ClaimAFriendShip(friendships[1]);

            // claiming beta
            Wrapper.Follow.CreateAFollow(friendships[2]);
            Wrapper.Follow.ClaimAFriendShip(friendships[2]);

            // verify every updated relationships
            VerifyFriendShipStateChanges<WrongFriendshipRemoval>(
                friendships,
                Wrapper.Follow.RemoveFriendship
            );
        }

        [TestMethod, ExpectedException(typeof(WrongFriendshipCreation))]
        public void ASuddenFriendshipIsOnlyPossibleFromNotExistingFollowing()
        {
            Account acc = Wrapper
                    .Account
                    .SearchByFullName("Ella Ditz").FirstOrDefault();

            Follow friendship = NullNormalized<Follow>(f =>
            {
                f.Following = JeanLucEmmanuel;
                f.Followed = acc;
            });

            Wrapper.Follow.CreateAFollow(friendship);
            Wrapper.Follow.CreateAFriendShip(friendship);
            Wrapper.Follow.CreateAFriendShip(friendship);
        }

        [TestMethod]
        public void StoppingAFollowIsOnlyPossibleIfThereIsOneFollowAlready()
        {
            // with a follow pass
            Follow friendship = MakeFollowingFullNames(
                "Ella Ditz", "Jaydu Bon-vin"
            );

            Wrapper.Follow.CreateAFollow(friendship);
            Wrapper.Follow.RemoveAFollow(friendship);

            Follow[] friendships = new Follow[] {

                // already removed
                friendship,

                MakeFollowingFullNames("Cezar Salad","Rogue Rogue")
            };

            // blocking
            Wrapper.Follow.MakeABlock(friendships[0]);

            // friends
            Wrapper.Follow.ClaimAFriendShip(friendships[1]);
            Wrapper.Follow.AcceptFriend(friendships[1]);

            // verify every updated relationships
            VerifyFriendShipStateChanges<WrongUnfollowingException>(
                friendships,
                Wrapper.Follow.RemoveAFollow
            );
        }

        private void VerifyFriendShipStateChanges<E>(Follow[] friendships, InvokeOnFollow method) 
            where E : Exception
        {
            int cpt = 0;
            foreach (Follow link in friendships)
            {
                try
                {
                    method(link);
                }
                catch (E)
                {
                    cpt++;
                }catch (Exception ex)
                {
                    throw ex;
                }
            }

            Assert.AreEqual(friendships.Length, cpt);
        }
    }

}
