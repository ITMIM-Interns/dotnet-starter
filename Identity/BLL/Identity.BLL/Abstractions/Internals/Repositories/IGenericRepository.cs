using Identity.Entity.Commons;

namespace Identity.BLL.Abstractions.Internals.Repositories
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey> where TKey : struct
    {
        Task Add(TEntity entity);
        Task Remove(TEntity entity);
        Task<TEntity?> GetByIdAsync(TKey id,bool hasTracked=false);
    }
}
