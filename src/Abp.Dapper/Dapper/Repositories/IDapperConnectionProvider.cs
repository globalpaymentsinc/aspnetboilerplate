using System.Data.Common;
using System.Threading.Tasks;
using Abp.Dapper.Filters.Action;
using Abp.Dapper.Filters.Query;
using Abp.Events.Bus.Entities;

namespace Abp.Dapper.Repositories
{
    public interface IDapperConnectionProvider<TDbContext> where TDbContext : System.Data.Entity.DbContext
    {
        DbConnection Connection { get; }

        Task<DbConnection> GetConnectionAsync();

        DbTransaction ActiveTransaction { get; }

        Task<DbTransaction> GetActiveTransactionAsync();

        IDapperQueryFilterExecuter DapperQueryFilterExecuter { get; set; }

        IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        IDapperActionFilterExecuter DapperActionFilterExecuter { get; set; }
    }
}