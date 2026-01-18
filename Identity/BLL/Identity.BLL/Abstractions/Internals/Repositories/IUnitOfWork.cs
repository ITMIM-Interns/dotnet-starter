namespace Identity.BLL.Abstractions.Internals.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveAsync();
    }
}
