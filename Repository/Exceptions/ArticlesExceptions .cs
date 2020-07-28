using Entities.EmmBlog.DataModelObjects;
using System;
using System.Text;

namespace Repository.Exceptions
{
    public class ArticlesExceptions : EmmBlogException<Article>
    {
        public class NoArticleFoundException : NullReferenceException
        {
            public NoArticleFoundException(string message) : base(message)
            { }

            public NoArticleFoundException(Article article)
                : base(ExpliciteNotFoundMessage(article,
                    new object[] { article.Slug, article.BlogId, article.VDepth }
                )
            )
            { }
        }

        public class TwiceAddingArticleException : InvalidOperationException
        {
            public TwiceAddingArticleException(string message) : base(message)
            { }

            public TwiceAddingArticleException(Article article)
                : base(ExpliciteDuplicationFoundMessage(article,
                    new object[] { article.Slug, article.BlogId, article.VDepth }
                )
            )
            { }
        }

        internal class NoReferencedArticleCanBeDeletedException : InvalidOperationException
        {
            public static string ExcpliciteCannotDelete(Article article)
            {
                string identifying = $"[key:{article.Slug}|{article.BlogId}|{article.VDepth}]";

                StringBuilder sb = new StringBuilder()
                    .Append($"\r\nThe comment given by {identifying} has some entities ")
                    .Append($"refering to it:");

                if (article.Comments != null)
                {
                    sb.Append($"\r\n\r\n-- Commenting --");
                    foreach (Comment comment in article.Comments)
                    {
                        string commenting = $"[key:{comment.Id}]";
                        sb.Append($"\r\n- {commenting}");
                    }
                }

                if (article.Shares != null)
                {
                    if (article.Comments != null)
                    {
                        sb.Append($"\r\n");
                    }
                    
                    sb.Append($"\r\n\r\n-- Sharing Blogs --");
                    foreach (Share share in article.Shares)
                    {
                        string sharingBlog = $"[key:{share.SharingBlogId}]";
                        sb.Append($"\r\n- {sharingBlog}");
                    }
                }

                sb
                    .Append("\r\nRemove the answering then proceed ")
                    .Append("or set the visibility of the desired comment to removed ")
                    .Append("by setting the visibilityId to 99");

                return sb.ToString();
            }

            public NoReferencedArticleCanBeDeletedException(string message) : base(message)
            { }

            public NoReferencedArticleCanBeDeletedException(Article article)
                : base(ExcpliciteCannotDelete(article))
            { }
        }
    }
}