using Entities;
using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataTransferObjects.In;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Repository;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Utilities;
using static Entities.EmmBlog.DataModelObjects.ReactionType;
using static Entities.EmmBlog.DataModelObjects.Status;
using static Entities.EmmBlog.DataModelObjects.Visibility;
using static UnitTestProject.ConfigurationHelper;
using static Utilities.Reflector;

namespace UnitTestProject.Tests
{
    [TestClass]
    public abstract class UnitTest
    {
        #region TYPES

        // Socials
        protected const int LIKE = (int)SocialReaction.Like;

        protected const int LOVE = (int)SocialReaction.Love;
        protected const int HAHA = (int)SocialReaction.Haha;
        protected const int ANGRY = (int)SocialReaction.Angry;
        protected const int CONFUSED = (int)SocialReaction.Confused;
        protected const int SURPRISED = (int)SocialReaction.Surprised;
        protected const int SAD = (int)SocialReaction.Sad;

        // Visibilities

        protected const int PUBLIC = (int)ContentVisibility.Public;
        protected const int FRIENDS = (int)ContentVisibility.Friends;
        protected const int PERSONAL = (int)ContentVisibility.Personal;
        protected static int VHIDDEN = (int)ContentVisibility.Hidden;
        protected const int REMOVED = (int)ContentVisibility.Removed;

        // Status

        protected const int Friend = (int)FollowStatus.Friend;
        protected const int Pending = (int)FollowStatus.PendingAlpha;
        protected const int NotFriend = (int)FollowStatus.Following;

        #endregion TYPES

        #region PROPERTIES

        protected static string HELLOWORLDPASS = "1594244d52f2d8c12b142bb61f47bc2eaf503d6d9ca8480cae9fcf112f66e4967dc5e8fa98285e36db8af1b8ffa8b84cb15e0fbcf836c3deb803c13f37659a60";
        public static RepositoryWrapper Wrapper { get; set; }
        protected static RepositoryContext Context { get; set; }


        public static Account Alex { get; private set; }
        public static Account JeanLucEmmanuel { get; private set; }
        public static Account Manu { get; private set; }
        public static Article ArticleZero { get; private set; }

        #endregion PROPERTIES

        #region INITIALIZATION

        /// <summary>
        /// Use AssemblyInitialize to run code before running the first test in the assembly.
        /// So every instance of UnitTest will initialize a wrapper and a context
        /// </summary>
        /// <param name="testContext"></param>
        [AssemblyInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            try
            {
                //string emmBlogPath = "D:/source/ASP/BlogServer/UnitTestProject2";
                string section = "BlogConfiguration";
                IConfiguration configuration = GetApplicationConfiguration(section);

                string connectionString = configuration["mysqlconnection:prod"];

                // To get connexionString out of configuration see also :
                // https://stackoverflow.com/questions/40845542/how-to-read-a-connectionstring-with-provider-in-net-core

                RepositoryContext context = new RepositoryContext(new DbContextOptionsBuilder().UseMySql(connectionString).Options);
                Wrapper = new RepositoryWrapper(context);
                Context = context;

                Scripts.Instance.SetStartupDatabase(connectionString);

                AddUsers();
                CreateBloggers();
                CreateArticles();
                CustomInit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        private static void CreateArticles()
        {
            Blog blog = Wrapper.Blog.GetBlogOfAccount(JeanLucEmmanuel.Id);
            ArticleZero = Wrapper.Article.CreateArticle(new Article()
            {
                Title = "An Article About Begining, Hurray !!!",
                Content = "What an interressant content !!!",
                Slug = "Article_Zero",
                Blog = blog
            });

            Wrapper.Article.CreateArticle(new Article()
            {
                Title = "A Way Later Article, Hurray !!!",
                Content = "What an interressant content !!!",
                Slug = "Article_After_Life",
                Blog = blog
            });
        }

        public static void CreateBloggers()
        {
            foreach (KeyValuePair<string, Account> row in new Dictionary<string, Account>(){
                {"EmmBlog", TransiantAccount("jean.luc.e.verhan@gmail.com", "Jean Luc Emmanuel", "Verhanneman") },
                {"ManuPremBlog" ,TransiantAccount("manmdyalw@gmail.com", "Manu", "Verhanneman") }
            })
            {
                Account found = Wrapper.Account.CreateAccount(MapperHelper.GetMapped<Account>(row.Value));
                Wrapper.Blog.CreateBlog(new Blog()
                {
                    BlogName = row.Key,
                    AccountId = found.Id
                });
            }

            Alex = GetAccountByFullName("Alex Bonaventure");
            Wrapper.Blog.CreateBlog(new Blog()
            {
                AccountId = Alex.Id,
                BlogName = "Alex Blog blog",
            });

            Account Cezar = GetAccountByFullName("Cezar Salad");
            Wrapper.Blog.CreateBlog(new Blog()
            {
                AccountId = Cezar.Id,
                BlogName = "Cezar blogger",
            });

            Account Selfe = GetAccountByFullName("Selfe Stiim");
            Wrapper.Blog.CreateBlog(new Blog()
            {
                AccountId = Selfe.Id,
                BlogName = "Selfe Steam engine blogger",
            });
            

             JeanLucEmmanuel = Wrapper.Account.GetAccountByMailAddress("jean.luc.e.verhan@gmail.com");
            Manu = Wrapper.Account.GetAccountByMailAddress("manmdyalw@gmail.com");
        }

        [TestInitialize]
        public void InitForAllTests()
        {
        }

        #endregion INITIALIZATION

        #region UNIT TESTS

        // TODO: Write here general test ...

        #endregion UNIT TESTS

        #region CLEANUPS

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void CleanupForAllTests() { }

        // Use AssemblyCleanup to run code after all tests in an assembly have run
        [AssemblyCleanup()]
        public static void MyClassCleanup() { }

        #endregion CLEANUPS

        protected static Account TransiantAccount(string mailAddress, string firstName, string lastName)
        {
            Account entity = new Account();
            entity.FirstName = firstName;
            entity.LastName = lastName;
            entity.Avatar = "Hello World So Cold.png";
            entity.DateOfBirth = new DateTime(1993, 01, 01);
            entity.VisibilityId = 1;

            Credential cred = entity.Credential = new Credential();
            cred.AccountId = entity.Id;
            cred.MailAddress = mailAddress;

            // helloworld
            cred.PassWord = HELLOWORLDPASS;

            return entity;
        }

        protected AccountForCreationDTO TransiantAccountForCreation(string mailAddress, string firstName, string lastName)
        {
            Account entity = TransiantAccount(mailAddress, firstName, lastName);
            return MapperHelper.GetMapped<AccountForCreationDTO>(entity);
        }

        protected static T NullNormalized<T>(Action<T> initializer = null) where T : class, new()
        {
            return new NullNormalizeFactory<T>(initializer).Instance;
        }

        protected static Comment ModifyComment(Comment original, Comment modification)
        {
            Reflector.Merge(modification, original, MergeOptions.KEEP_TARGET);
            return Wrapper.Comment.ChangeComment(modification);
        }

        protected static Comment ChainModify(string[] queue, Comment source)
        {
            foreach (string modif in queue)
            {
                source = ModifyComment(source, NullNormalized<Comment>(c =>
                {
                    c.WriteDate = DateTime.Now.AddHours(1);
                    c.Content = modif;
                }));
            }
            return source;
        }

        protected Article GetArticleLike(Article like)
        {
            return Wrapper.Article.GetArticle(like);
        }

        protected Comment GetCommentLike(Comment comment)
        {
            return Wrapper.Comment.GetComment(comment);
        }

        protected static void CustomInit()
        {
            var mth = new StackTrace().GetFrame(1).GetMethod();
            Console.WriteLine(mth.ReflectedType.Name + " : CustomInit method not implemented");
        }

        public static void AddUsers()
        {
            Account[] accs = new[]{
                TransiantAccount("j.doe@gmail.com", "John", "Doe"),
                TransiantAccount("j.fehtou@gmail.com", "Jessica", "Fehtou"),
                TransiantAccount("j.bonvin@gmail.com", "Jaydu", "Bon-vin"),
                TransiantAccount("a.bonaventure@gmail.com", "Alex", "Bonaventure"),
                TransiantAccount("j.gnack@gmail.com", "Jacko", "Gnack"),

                TransiantAccount("t.ato@gmail.com", "Tom", "Ato"),
                TransiantAccount("c.salad@gmail.com", "Cezar", "Salad"),
                TransiantAccount("r.beaf@gmail.com", "Rose", "Beaf"),
                TransiantAccount("c.doe@gmail.com", "Crishian", "Doe"),
                TransiantAccount("s.stiim@gmail.com", "Selfe", "Stiim"),

                TransiantAccount("r.rogue@gmail.com", "Rogue", "Rogue"),
                TransiantAccount("j.Gustavo@gmail.com", "Jules", "Gustavo"),
                TransiantAccount("g.Gomez@gmail.com", "Garcia", "Gomez"),
                TransiantAccount("m.kojiro@gmail.com", "Musashi", "Kojiro"),
                TransiantAccount("m.oeth@gmail.com", "Mucat", "Oeth"),

                TransiantAccount("m.ado@gmail.com", "Mick", "Ado"),
                TransiantAccount("l.rheto@gmail.com", "Lamas", "Retho"),
                TransiantAccount("e.ditz@gmail.com", "Ella", "Ditz"),
                TransiantAccount("l.morning-stark@gmail.com", "Lucilia", "Morning-Stark"),

                TransiantAccount("r.sansamy@hotmail.com", "Rémy", "Sansamy")
            };

            foreach (Account acc in accs)
            {
                Wrapper.Account.CreateAccount(acc);
            }
        }

        protected Follow MakeFollowing(string mailFollowed, string mailFollowing)
        {
            return NullNormalized<Follow>(f =>
            {
                f.Following = Wrapper.Account.GetAccountByMailAddress(mailFollowing);
                f.Followed = Wrapper.Account.GetAccountByMailAddress(mailFollowed);
            });
        }
        protected Follow MakeFollowing(Account followed, Account following)
        {
            return NullNormalized<Follow>(f =>
            {
                f.Following = following;
                f.Followed = followed;
            });
        }

        protected static Account GetAccountByFullName(string fullNameFollowed)
        {
            return Wrapper
                    .Account
                    .SearchByFullName(fullNameFollowed).FirstOrDefault();
        }

        protected Follow MakeFollowingFullNames(
            string fullNameFollowed, 
            string fullNameFollowing
        )
        {
            Account followed = Wrapper
                    .Account
                    .SearchByFullName(fullNameFollowed).FirstOrDefault();

            Account following = Wrapper
                    .Account
                    .SearchByFullName(fullNameFollowing).FirstOrDefault();

            return NullNormalized<Follow>(f =>
            {
                f.Following = following;
                f.Followed = followed;
            });
        }

        protected Follow MakeFollowingToAccount(
            Account account,
            string fullNameFollowing
        )
        {
            Account following = Wrapper
                    .Account
                    .SearchByFullName(fullNameFollowing).FirstOrDefault();

            return NullNormalized<Follow>(f =>
            {
                f.Following = following;
                f.Followed = account;
            });
        }

        protected Follow MakeFollowingToAccount(
            string fullNameFollowed, Account account
        )
        {
            Account followed = Wrapper
                    .Account
                    .SearchByFullName(fullNameFollowed).FirstOrDefault();

            return NullNormalized<Follow>(f =>
            {
                f.Following = account;
                f.Followed = followed;
            });
        }
    }
}