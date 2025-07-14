using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using MongoDB.Driver;

namespace Abp.MongoDb.Repositories
{
    /// <summary>
    /// Implements IRepository for MongoDB.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    public class MongoDbRepositoryBase<TEntity> : MongoDbRepositoryBase<TEntity, int>, IRepository<TEntity>
        where TEntity : class, IEntity<int>
    {
        public MongoDbRepositoryBase(IMongoDatabaseProvider databaseProvider)
            : base(databaseProvider)
        {
        }
    }

    /// <summary>
    /// Implements IRepository for MongoDB.
    /// </summary>
    /// <typeparam name="TEntity">Type of the Entity for this repository</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key of the entity</typeparam>
    public class MongoDbRepositoryBase<TEntity, TPrimaryKey> : AbpRepositoryBase<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        public virtual IMongoDatabase Database
        {
            get { return _databaseProvider.Database; }
        }

        public virtual IMongoCollection<TEntity> Collection
        {
            get
            {
                return _databaseProvider.Database.GetCollection<TEntity>(typeof(TEntity).Name);
            }
        }

        private readonly IMongoDatabaseProvider _databaseProvider;

        public MongoDbRepositoryBase(IMongoDatabaseProvider databaseProvider)
        {
            _databaseProvider = databaseProvider;
        }

        public override IQueryable<TEntity> GetAll()
        {
            return Collection.AsQueryable();
        }

        public override Task<IQueryable<TEntity>> GetAllAsync()
        {
            return Task.FromResult(Collection.AsQueryable());
        }

        public override TEntity Get(TPrimaryKey id)
        {
            var entity = Collection.Find(e => e.Id.Equals(id)).FirstOrDefault();
            if (entity == null)
            {
                throw new EntityNotFoundException("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }
            return entity;
        }
    
        public override TEntity FirstOrDefault(TPrimaryKey id)
        {
            return Collection.Find(e => e.Id.Equals(id)).FirstOrDefault();
        }

        public override TEntity Insert(TEntity entity)
        {
            Collection.InsertOne(entity);
            return entity;
        }
    
        public override TEntity Update(TEntity entity)
        {
            Collection.ReplaceOne(e => e.Id.Equals(entity.Id), entity);
            return entity;
        }

        public override void Delete(TEntity entity)
        {
            Delete(entity.Id);
        }

        public override void Delete(TPrimaryKey id)
        {
            Collection.DeleteOne(e => e.Id.Equals(id));
        }

        public override async Task<TEntity> GetAsync(TPrimaryKey id)
        {
            var entity = await Collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync();
            if (entity == null)
            {
                throw new EntityNotFoundException("There is no such an entity with given primary key. Entity type: " + typeof(TEntity).FullName + ", primary key: " + id);
            }
            return entity;
        }

        public override async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await Collection.Find(e => e.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public override async Task<TEntity> InsertAsync(TEntity entity)
        {
            await Collection.InsertOneAsync(entity);
            return entity;
        }

        public override async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await Collection.ReplaceOneAsync(e => e.Id.Equals(entity.Id), entity);
            return entity;
        }

        public override async Task DeleteAsync(TPrimaryKey id)
        {
            await Collection.DeleteOneAsync(e => e.Id.Equals(id));
        }
    }
}
