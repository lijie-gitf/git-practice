using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using CoreEntirty.Extensions;

namespace CoreEntirty
{
    public class CoreSqlRepository : BaseRepository, ICoreRepository
    {
       

        public CoreSqlRepository(CoreDbContext dbContext)
        {
         
            DbContext = dbContext;
        }
      
        public void Dispose()
        {
            if (this.DbContext.Database != null)
            {

                Close();
            }
        }
        public override ICoreRepository BeginTrans()
        {
            if (DbContext.Database.CurrentTransaction == null)
            {
                DbContext.Database.BeginTransaction();
            }
            return this;
        }

        public override async Task<ICoreRepository> BeginTransAsync()
        {
            if (DbContext.Database.CurrentTransaction == null)
            {
               await DbContext.Database.BeginTransactionAsync();
            }
            return this;
        }



        public DataTable SqlQuery(string sql, SqlParameter[] parameters)
        {
            return DbContext.Database.SqlQuery(sql, parameters);
        }

        public async Task<DataTable> SqlQueryAsync(string sql, SqlParameter[] parameters)
        {
            return await DbContext.Database.SqlQueryAsync(sql, parameters);
        }

        public IEnumerable<Tntity> SqlQuery<Tntity>(string sql, SqlParameter[] parameters) where Tntity : BaseEntity, new()
        {
            return DbContext.Database.SqlQuery<Tntity>(sql, parameters);
        }

        public async Task<IEnumerable<Tntity>> SqlQueryAsync<Tntity>(string sql, SqlParameter[] parameters) where Tntity : BaseEntity, new()
        {
            return await DbContext.Database.SqlQueryAsync<Tntity>(sql, parameters);
        }

        public DataTable SqlQuery(string sql)
        {
            return DbContext.Database.SqlQuery(sql);
        }

        public async Task<DataTable> SqlQueryAsync(string sql)
        {
            return await DbContext.Database.SqlQueryAsync(sql);
        }
    }
}
