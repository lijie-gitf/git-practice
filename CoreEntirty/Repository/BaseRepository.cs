using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntirty
{
    public abstract class BaseRepository
    {
        #region 属性
        public virtual CoreDbContext DbContext { get; set; }



        /// <summary>
        /// 数据库连接
        /// </summary>
        private string connectionString { get; set; }

        public string ConnectionString
        {
            get
            {
                if (string.IsNullOrWhiteSpace(connectionString))
                {
                    connectionString = DbContext.Database.GetDbConnection()?.ConnectionString;
                }
                return connectionString;
            }
        }

        //事务对象
        private DbTransaction dbTransaction { get; set; }
        public DbTransaction DbTransaction
        {
            get
            {
                return dbTransaction ?? DbContext.Database.CurrentTransaction?.GetDbTransaction();
            }
            set
            {
                dbTransaction = value;
                DbContext.Database.UseTransaction(dbTransaction);
            }
        }
        #endregion

        #region 事务
        public abstract ICoreRepository BeginTrans();

        public virtual void Commit()
        {
            DbContext.Database.CommitTransaction();
            //释放
            DbContext.Database.CurrentTransaction?.Dispose();
        }

        public virtual void Rollback()
        {
            DbContext.Database.RollbackTransaction();
            DbContext.Database.CurrentTransaction?.Dispose();
        }

        public virtual void Close()
        {
            DbContext.Database.CloseConnection();
            DbContext.Dispose();
        }

        public abstract Task<ICoreRepository> BeginTransAsync();

        public virtual async Task CommitAsync()
        {
            await DbContext.Database.CommitTransactionAsync();
            //释放
            DbContext.Database.CurrentTransaction?.Dispose();

        }

        public virtual async Task RollbackAsync()
        {
            await DbContext.Database.RollbackTransactionAsync();
            DbContext.Database.CurrentTransaction?.Dispose();
        }

        public virtual async Task CloseAsync()
        {
            await DbContext.Database.CloseConnectionAsync();
            DbContext.Dispose();
        }

        public virtual void ExecuteTrans(Func<ICoreRepository, bool> Func, Action<Exception> Rollback = null)
        {
            ICoreRepository repository = null;
            try
            {
                if (Func != null)
                {
                    repository = this.BeginTrans();
                    var res = Func(repository);
                    if (res)
                    {
                        repository.Commit();
                    }
                    else
                    {
                        repository.Rollback();
                    }
                }

            }
            catch (Exception ex)
            {
                repository?.Rollback();

                if (Rollback != null)
                    Rollback(ex);
                else
                    throw;
            }
        }

        public virtual async Task ExecuteTransAsync(Func<ICoreRepository, bool> Func, Action<Exception> Rollback = null)
        {
            ICoreRepository repository = null;
            try
            {
                if (Func != null)
                {
                    repository = await this.BeginTransAsync();
                    var res = Func(repository);
                    if (res)
                    {
                        repository.Commit();
                    }
                    else
                    {
                        repository.Rollback();
                    }
                }

            }
            catch (Exception ex)
            {
                repository?.Rollback();

                if (Rollback != null)
                    Rollback(ex);
                else
                    throw;
            }
        }
        #endregion

        #region 基本方法
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回受影响行数</returns>
        public virtual int ExecuteBySql(string sql)
        {
            return DbContext.Database.ExecuteSqlRaw(sql);
        }
        public virtual async Task<int> ExecuteBySqlAsyns(string sql)
        {
            return await DbContext.Database.ExecuteSqlRawAsync(sql);
        }

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="parameter">对应参数</param>
        /// <returns>返回受影响行数</returns>
        public virtual int ExecuteBySql(string sql, params object[] parameter)
        {
            return DbContext.Database.ExecuteSqlRaw(sql, parameter);
        }

        public virtual async Task<int> ExecuteBySqlAsyns(string sql, params object[] parameter)
        {
            return await DbContext.Database.ExecuteSqlRawAsync(sql, parameter);
        }
        /// <summary>
        /// 根据ID获取一个实体对象
        /// </summary>
        /// <typeparam name="Tntity"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>

        public virtual Tntity GetById<Tntity>(int Id) where Tntity : BaseEntity
        {
            return DbContext.Set<Tntity>().Where(p => p.Id.Equals(Id)).FirstOrDefault();
        }

        public virtual async Task<Tntity> GetByIdAsync<Tntity>(int Id) where Tntity : BaseEntity
        {
            return await DbContext.Set<Tntity>().Where(p => p.Id.Equals(Id)).FirstOrDefaultAsync();
        }
        public virtual IEnumerable<Tntity> Get<Tntity>(Expression<Func<Tntity, bool>> predicate) where Tntity : BaseEntity
        {
            return DbContext.Set<Tntity>().Where(predicate).ToList();
        }
        public virtual async Task<IEnumerable<Tntity>> GetAsync<Tntity>(Expression<Func<Tntity, bool>> predicate) where Tntity : BaseEntity
        {
            return await DbContext.Set<Tntity>().Where(predicate).ToListAsync();
        }
        public virtual void Update<Tntity>(Tntity tntity) where Tntity : BaseEntity
        {
            DbContext.Set<Tntity>().Update(tntity);
            DbContext.SaveChanges();
        }
        public virtual async Task UpdateAsync<Tntity>(Tntity tntity) where Tntity : BaseEntity
        {
            DbContext.Set<Tntity>().Update(tntity);
           await DbContext.SaveChangesAsync();
        }

        #endregion



    }
}
