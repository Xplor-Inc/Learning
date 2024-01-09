using XploringMe.Core.Models.Entities.Users;

namespace XploringMe.Core.Interfaces.Data;
public interface IDataContext<TUser> : IContext
        where TUser : User
{
    IQueryable<User>    Users   { get; }
}