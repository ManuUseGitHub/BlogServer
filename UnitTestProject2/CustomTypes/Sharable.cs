using Entities.EmmBlog.DataModelObjects;
using UnitTestProject.Tests;
using UnitTestProject.Tests.ConcreteTests;

namespace UnitTestProject.CustomTypes
{
    public class Sharable : Article , ICanCopy<Sharable>
    {
        public Sharable(string userMail, string sharerMail, Article article) : this(userMail, sharerMail)
        {
            Utilities.Reflector.Merge(this, article);
            Blog = BlogSource;
        }

        public Sharable(string userMail, string sharerMail)
        {
            Account acc = UnitTest.Wrapper.Account.GetAccountByMailAddress(userMail);
            Account acc2 = UnitTest.Wrapper.Account.GetAccountByMailAddress(sharerMail);
            BlogSource = UnitTest.Wrapper.Blog.GetBlogOfAccount(acc.Id);
            BlogDestin = UnitTest.Wrapper.Blog.GetBlogOfAccount(acc2.Id);

            BlogId = BlogSource.Id;
            VDepth = 0;

            Utilities.Reflector.Merge(this, ArticleUTest.getAFirstArticle(BlogSource.Id));
        }

        public Sharable MergeWith(Article article)
        {
            Utilities.Reflector.Merge(this, article);
            return this;
        }


        public Blog BlogSource { get; }
        public Blog BlogDestin { get; }

        public Sharable GetCopy()
        {
            return MergeWith(new Article());
        }
    }
}
