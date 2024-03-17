using System.Data;
using System.Threading.Tasks;
using Abp.Dapper.Filters.Action;
using Abp.Dapper.Filters.Query;
using Abp.Events.Bus.Entities;

namespace Abp.Dapper.Repositories
{
    public interface IDapperRepository
    {
        IDbConnection Connection { get; }

        Task<IDbConnection> GetConnectionAsync();

        IDbTransaction ActiveTransaction { get; }

        Task<IDbTransaction> GetActiveTransactionAsync();

        IDapperQueryFilterExecuter DapperQueryFilterExecuter { get; set; }

        IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        IDapperActionFilterExecuter DapperActionFilterExecuter { get; set; }
    }
}