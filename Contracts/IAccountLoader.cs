using Entities.EmmBlog.DataModelObjects;
using Entities.EmmBlog.DataModelObjects.dependentInterfaces;

namespace Contracts
{
    public interface IAccountLoader
    {
        void LoadCredentials(Account account);
        void LoadAccount(IHaveAccount havingAccount);
    }
}