using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataTransferObjects.In;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository.Exceptions;
using System;
using Utilities;
using static Repository.Exceptions.AccountsExceptions;

namespace UnitTestProject.Tests.ConcreteTests
{
    [TestClass]
    public class AccountUTest : UnitTest
    {
        [TestMethod, ExpectedException(typeof(FriendlessHiddingException))]
        public void AFriendlessAccountCannotSetItsVisibilityToFriends()
        {
            Account Remy = Wrapper.Account.GetAccountByMailAddress("r.sansamy@hotmail.com");

            Reflector.Merge(Remy,new Account()
            {
                VisibilityId = FRIENDS
            });

            Wrapper.Account.ChangeInfo(Remy);
        }

        [TestMethod]
        public void AnAccountWithOneFriendCanSetItsVisibilityToFriends()
        {
            Reflector.Merge(Alex, new Account()
            {
                VisibilityId = FRIENDS
            });

            Follow relation = new Follow()
            {
                Followed = Alex,
                Following = GetAccountByFullName("Mick Ado")
            };

            Wrapper.Follow.ClaimAFriendShip(relation) ;
            Wrapper.Follow.AcceptFriend(relation);

            Wrapper.Account.ChangeInfo(Alex);
        }

        [TestMethod]
        public void AnAddedAccountCanBeFound()
        {
            Account transiant = TransiantAccount("test1@gmail.com", "Manu", "Verh");
            var acc = Wrapper.Account.CreateAccount(transiant);

            Account acc2 = Wrapper.Credential.GetAccountWithCredential(transiant.Id);

            Assert.AreEqual(acc.Id, acc2.Id);
        }

        [TestMethod]
        public void AMappedDTOKeepsTheSameGuid()
        {
            AccountForCreationDTO dto = new AccountForCreationDTO();
            Account account = MapperHelper.GetMapped<Account>(dto);

            Assert.AreEqual(dto.Id, account.Id);
        }
        [TestMethod]
        public void AnAccountCanSubscribesToABlog()
        {
            Account transiant = TransiantAccount("tjasmin@gmail.com", "Théo", "Jasmin");
            Account found = Wrapper.Account.CreateAccount(MapperHelper.GetMapped<Account>(transiant));

            Account acc = Wrapper.Account.GetAccountByMailAddress("jean.luc.e.verhan@gmail.com");
            Blog blog = Wrapper.Blog.GetBlogOfAccount(acc.Id);

            Subscribe subscription = Wrapper.Blog.CreateSubscription(transiant.Id, blog.Id);

            Assert.IsTrue(
                found.Id.Equals(subscription.AccountId) &&
                blog.Id.Equals(subscription.BlogId)
            );
        }

        #region should fail
        [TestMethod, ExpectedException(typeof(NotFoundAccountException))]
        public void CannotFindANonExistingAccountId()
        {
            Wrapper.Credential.GetAccountWithCredential(new Guid("00000000-0000-0000-0000-000000000000"));
        }
        [TestMethod, ExpectedException(typeof(NonPublicVisibilityException))]
        public void ANonPublicAccountThrowsAnException()
        {
            Account transiant = TransiantAccount("err.non.public@gmail.com", "Ernest", "Nopublic");

            // not available visibility
            transiant.VisibilityId = 2;

            Wrapper.Account.CreateAccount(transiant);
        }

        [TestMethod, ExpectedException(typeof(BornInFuturException))]
        public void ABornInFuturAccountThrowsAnException()
        {
            Account transiant = TransiantAccount("jcafeh@gmail.com", "Jessica", "Feh");

            // born in 1 month later
            transiant.DateOfBirth = DateTime.Now.AddDays(30);

            Wrapper.Account.CreateAccount(MapperHelper.GetMapped<Account>(transiant));
        }

        [TestMethod, ExpectedException(typeof(UnpairedAccountAndCredentialException))]
        public void AnNewAccountWithoutCredentialsThrows()
        {
            Account transiant = TransiantAccount("err.without.credential@gmail.com", "Ernest", "Credential");

            transiant.Credential = null;

            // throws
            Wrapper.Account.CreateAccount(transiant);
        }
        #endregion
    }
}
