using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Phonebook.Shared.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Phonebook.Shared.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {

        protected readonly IMongoCollection<T> _collection;
        protected readonly DBSettings _databaseSetting;

        public Repository(IOptions<DBSettings> databaseSetting)
        {
            _databaseSetting = databaseSetting.Value;
            var client = new MongoClient(_databaseSetting.ConnectionString);
            var database = client.GetDatabase(_databaseSetting.DatabaseName);
            _collection = database.GetCollection<T>(typeof(T).Name.ToLowerInvariant());
        }

        public virtual IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            return predicate == null
                ? _collection.AsQueryable()
                : _collection.AsQueryable().Where(predicate);
        }

        public virtual Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return _collection.Find(predicate).FirstOrDefaultAsync();
        }

        public virtual Task<T> GetAsync(string id)
        {
            return _collection.Find(x => x.UUID == id).FirstOrDefaultAsync();
        }
        public virtual Task<List<T>> GetListWithFiltersAsync(Expression<Func<T, bool>> predicate)
        {
            var data = _collection.AsQueryable<T>();
            data = data.Where(predicate);

            return data.ToListAsync();

        }

        public virtual async Task<T> AddAsync(T entity)
        {
            var options = new InsertOneOptions { BypassDocumentValidation = false };
            await _collection.InsertOneAsync(entity, options);
            return entity;
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<T> entities)
        {
            var options = new BulkWriteOptions { IsOrdered = false, BypassDocumentValidation = false };
            return (await _collection.BulkWriteAsync((IEnumerable<WriteModel<T>>)entities, options)).IsAcknowledged;
        }

        public virtual async Task<T> UpdateAsync(string id, T entity)
        {
            return await _collection.FindOneAndReplaceAsync(x => x.UUID == id, entity);
        }

        public virtual async Task<T> UpdateAsync(T entity, Expression<Func<T, bool>> predicate)
        {
            return await _collection.FindOneAndReplaceAsync(predicate, entity);
        }

        public virtual async Task<T> DeleteAsync(T entity)
        {
            return await _collection.FindOneAndDeleteAsync(x => x.UUID == entity.UUID);
        }

        public virtual async Task<T> DeleteAsync(string id)
        {
            return await _collection.FindOneAndDeleteAsync(x => x.UUID == id);
        }

        public virtual async Task<T> DeleteAsync(Expression<Func<T, bool>> filter)
        {
            return await _collection.FindOneAndDeleteAsync(filter);
        }
        //public virtual async Task<T> DeleteAllAsync(Expression<Func<T, bool>> filter)
        //{
        //    return await _collection.DeleteManyAsync(filter);
        //}
    }
}
