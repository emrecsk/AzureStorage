using Microsoft.Azure.Cosmos.Table;
using System.Linq.Expressions;

namespace AzureStrorageLibrary.Services
{
    public class TableStorage<TEntity> : INoSqlStorage<TEntity> where TEntity : TableEntity, new() // The given class must be inherited from TableEntity and create a new instance of it
    {
        private readonly CloudTableClient _table;
        private readonly CloudTable _cloudTable;

        public TableStorage()
        {
            _table = CloudStorageAccount.Parse(ConnectionString.Con).CreateCloudTableClient();
            _cloudTable = _table.GetTableReference(typeof(TEntity).Name);
            _cloudTable.CreateIfNotExistsAsync();
        }

        public async Task<TEntity?> Create(TEntity entity)
        {
            var insertOperation = TableOperation.InsertOrMerge(entity);
            var result = await _cloudTable.ExecuteAsync(insertOperation);
            
            return result.Result as TEntity;
        }

        public async Task Delete(string partitionKey, string rowKey)
        {
            var deleteOperation = TableOperation.Delete(new TEntity { PartitionKey = partitionKey, RowKey = rowKey, ETag = "*" });
            await _cloudTable.ExecuteAsync(deleteOperation);            
        }

        public async Task<TEntity?> Get(string partitionKey, string rowKey)
        {
            var operation = TableOperation.Retrieve<TEntity>(partitionKey, rowKey);
            var result = await _cloudTable.ExecuteAsync(operation);
            
            return result.Result as TEntity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _cloudTable.CreateQuery<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> filter)
        {
            return _cloudTable.CreateQuery<TEntity>().Where(filter).AsQueryable();
        }

        public async Task<TEntity?> Update(TEntity entity)
        {
            var operation = TableOperation.Replace(entity);
            var result = await _cloudTable.ExecuteAsync(operation);
            return result.Result as TEntity;
        }
    }
}