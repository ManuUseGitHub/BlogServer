using Entities.EmmBlog.DataModelObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTestProject.Tests.ConcreteTests
{
    [TestClass]
    public class ConnexionUTest : UnitTest
    {
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
        }

        [TestMethod]
        public void AnAccountCanSignIn()
        {
            string password = "1594244d52f2d8c12b142bb61f47bc2eaf503d6d9ca8480cae9fcf112f66e4967dc5e8fa98285e36db8af1b8ffa8b84cb15e0fbcf836c3deb803c13f37659a60";
            Wrapper.Credential.ConnectUser("j.fehtou@gmail.com", password);
        }

        [TestMethod]
        public void ASignedInAccountCreateAConnexion()
        {
            string password = "1594244d52f2d8c12b142bb61f47bc2eaf503d6d9ca8480cae9fcf112f66e4967dc5e8fa98285e36db8af1b8ffa8b84cb15e0fbcf836c3deb803c13f37659a60";
            Account acc2 = Wrapper.Credential.ConnectUser("j.fehtou@gmail.com", password).Account;

            int count = Wrapper.Connexion.GetAccountConnexions(acc2.Id).Count;
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void AConnexionCanBeInvalidated()
        {
            string password = "1594244d52f2d8c12b142bb61f47bc2eaf503d6d9ca8480cae9fcf112f66e4967dc5e8fa98285e36db8af1b8ffa8b84cb15e0fbcf836c3deb803c13f37659a60";
            Connexion con = Wrapper.Credential.ConnectUser("j.doe@gmail.com", password);

            Wrapper.Connexion.InvalidateConnexion(con.Id, con.AccountId);
            Wrapper.Connexion.GetConnexion(con.Id, con.AccountId);

            Assert.IsFalse(con.Valid);
        }

        [TestMethod]
        public void AllConnexionsAreDiscartedIfInvalid()
        {
            string password = "1594244d52f2d8c12b142bb61f47bc2eaf503d6d9ca8480cae9fcf112f66e4967dc5e8fa98285e36db8af1b8ffa8b84cb15e0fbcf836c3deb803c13f37659a60";
            Connexion con = Wrapper.Credential.ConnectUser("a.bonaventure@gmail.com", password);

            int count1 = Wrapper.Connexion.GetConnexionCount(con.AccountId);

            Wrapper.Connexion.InvalidateConnexion(con.Id, con.AccountId);
            ICollection<Connexion> discarded = Wrapper.Connexion.DiscardConnexions(con.AccountId);

            int count2 = Wrapper.Connexion.GetConnexionCount(con.AccountId);

            Assert.AreNotEqual(count1, count2);
            Assert.AreNotEqual(discarded.Count, 0);
            Assert.AreEqual(count2, 0);
        }

    }
}
