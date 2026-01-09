using MiniApp.Models.Commons;

namespace MiniApp.BLL.Abstractions.Internals.Writes
{
    public interface IGenericWriteRepository<TEntity,Tkey> where TEntity : BaseEntity<Tkey> where Tkey : struct 
    {
        Task Add(TEntity entity);
        Task Remove(TEntity entity);
    }
}
