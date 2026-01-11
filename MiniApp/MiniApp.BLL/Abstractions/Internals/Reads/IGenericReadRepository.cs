using MiniApp.Models.Commons;

namespace MiniApp.BLL.Abstractions.Internals.Reads
{
    public interface IGenericReadRepository<TEntity,TKey> where TEntity : BaseEntity<TKey> where TKey : struct
    {
        Task<TEntity?> GetByIdAsync(TKey id,bool hasTracked=false);
    }
}
