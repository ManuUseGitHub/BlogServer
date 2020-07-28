namespace Contracts
{
    public interface IRepoLoaderWrapper
    {
        IArticleLoader Article { get; }
        IBlogLoader Blog { get; }
        IAccountLoader Account { get; }
        ISocialLoader Social { get; }
    }
}
