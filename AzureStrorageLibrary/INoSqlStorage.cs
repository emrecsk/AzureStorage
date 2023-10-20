using System.Linq.Expressions;

namespace AzureStrorageLibrary
{
    public interface INoSqlStorage<TEntity>
    {
        Task<TEntity?> Create(TEntity entity);
        Task<TEntity?> Update(TEntity entity);
        Task Delete(string partitionKey, string rowKey);
        Task<TEntity?> Get(string partitionKey, string rowKey);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter);
    }
}
