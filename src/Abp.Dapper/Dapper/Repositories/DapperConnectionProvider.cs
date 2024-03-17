using System.Data.Common;
using System.Threading.Tasks;
using Abp.Dapper.Filters.Action;
using Abp.Dapper.Filters.Query;
using Abp.Data;
using Abp.Events.Bus.Entities;
using Abp.MultiTenancy;

namespace Abp.Dapper.Repositories
{

    public abstract class DapperConnectionProvider<TDbContext> : IDapperConnectionProvider<TDbContext> where TDbContext : System.Data.Entity.DbContext
    {

        public virtual MultiTenancySides? MultiTenancySide { get; set; }

        private readonly IActiveTransactionProvider _activeTransactionProvider;

        protected DapperConnectionProvider(IActiveTransactionProvider activeTransactionProvider)
        {
            _activeTransactionProvider = activeTransactionProvider;
        }

        private ActiveTransactionProviderArgs ActiveTransactionProviderArgs =>
            new ActiveTransactionProviderArgs
            {
                ["ContextType"] = typeof(TDbContext),
                ["MultiTenancySide"] = MultiTenancySide
            };

        public virtual IDapperQueryFilterExecuter DapperQueryFilterExecuter { get; set; } = NullDapperQueryFilterExecuter.Instance;

        public virtual IEntityChangeEventHelper EntityChangeEventHelper { get; set; } = NullEntityChangeEventHelper.Instance;

        public virtual IDapperActionFilterExecuter DapperActionFilterExecuter { get; set; } = NullDapperActionFilterExecuter.Instance;

        public virtual DbConnection Connection =>
            (DbConnection)_activeTransactionProvider.GetActiveConnection(ActiveTransactionProviderArgs);

        public virtual async Task<DbConnection> GetConnectionAsync()
        {
            var connection = await _activeTransactionProvider.GetActiveConnectionAsync(ActiveTransactionProviderArgs);
            return (DbConnection)connection;
        }

        public virtual DbTransaction ActiveTransaction =>
            (DbTransaction)_activeTransactionProvider.GetActiveTransaction(ActiveTransactionProviderArgs);

        public virtual async Task<DbTransaction> GetActiveTransactionAsync()
        {
            var transaction = await _activeTransactionProvider.GetActiveTransactionAsync(ActiveTransactionProviderArgs);
            return (DbTransaction)transaction;
        }
    }
}
