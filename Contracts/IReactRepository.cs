using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;

namespace Contracts
{
    public interface IReactRepository
    {
        public React React(React reaction);

        React GetReact<T>(T entity, Account account);
        React RemoveReaction<T>(T entity, Account account);
        void RemoveReactionsOf<T>(T entity);
        void UpgradReactionsOf<T>(T entity);
        public void SetTop3ReactionCounts(IHaveReactions item);
    }
}