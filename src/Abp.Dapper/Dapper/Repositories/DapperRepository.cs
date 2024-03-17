using System.Data;
using System.Threading.Tasks;
using Abp.Dapper.Filters.Action;
using Abp.Dapper.Filters.Query;
using Abp.Data;
using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.MultiTenancy;
using Abp.Reflection.Extensions;

namespace Abp.Dapper.Repositories
{

    public class DapperRepository<TDbContext, TEntity> : IDapperRepository, ITransientDependency where TDbContext : System.Data.Entity.DbContext
    {

        public MultiTenancySides? MultiTenancySide { get; }

        private readonly IActiveTransactionProvider _activeTransactionProvider;

        public DapperRepository(IActiveTransactionProvider activeTransactionProvider)
        {
            _activeTransactionProvider = activeTransactionProvider;

            EntityChangeEventHelper = NullEntityChangeEventHelper.Instance;
            DapperQueryFilterExecuter = NullDapperQueryFilterExecuter.Instance;
            DapperActionFilterExecuter = NullDapperActionFilterExecuter.Instance;

            var attr = typeof(TEntity).GetSingleAttributeOfTypeOrBaseTypesOrNull<MultiTenancySideAttribute>();
            if (attr != null)
            {
                MultiTenancySide = attr.Side;
            }
        }

        private ActiveTransactionProviderArgs ActiveTransactionProviderArgs =>
            new ActiveTransactionProviderArgs
            {
                ["ContextType"] = typeof(TDbContext),
                ["MultiTenancySide"] = MultiTenancySide
            };

        public IDapperQueryFilterExecuter DapperQueryFilterExecuter { get; set; }

        public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        public IDapperActionFilterExecuter DapperActionFilterExecuter { get; set; }

        public virtual IDbConnection Connection =>
            _activeTransactionProvider.GetActiveConnection(ActiveTransactionProviderArgs);

        public virtual async Task<IDbConnection> GetConnectionAsync()
        {
            return await _activeTransactionProvider.GetActiveConnectionAsync(ActiveTransactionProviderArgs);
        }

        public virtual IDbTransaction ActiveTransaction =>
            _activeTransactionProvider.GetActiveTransaction(ActiveTransactionProviderArgs);

        public virtual async Task<IDbTransaction> GetActiveTransactionAsync()
        {
            return await _activeTransactionProvider.GetActiveTransactionAsync(ActiveTransactionProviderArgs);
        }
    }
}
