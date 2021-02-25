using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntirty
{
  public  interface ICoreRepository:IDisposable
    {
        /// <summary>
        /// 根据Id获取实体对象
        /// </summary>
        /// <typeparam name="Tntity"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        Tntity GetById<Tntity>(int Id) where Tntity : BaseEntity;

        /// <summary>
        /// 根据Id获取实体对象，异步
        /// </summary>
        /// <typeparam name="Tntity"></typeparam>
        /// <param name="Id"></param>
        /// <returns></returns>
        Task<Tntity> GetByIdAsync<Tntity>(int Id) where Tntity : BaseEntity;

        /// <summary>
        /// 获取一个实体对象，表达式形式
        /// </summary>
        /// <typeparam name="Tntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IEnumerable<Tntity> Get<Tntity>(Expression<Func<Tntity, bool>> predicate) where Tntity : BaseEntity;

        /// <summary>
        /// 获取一个实体对象，表达式形式，异步
        /// </summary>
        /// <typeparam name="Tntity"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<Tntity>> GetAsync<Tntity>(Expression<Func<Tntity, bool>> predicate) where Tntity : BaseEntity;

        /// <summary>
        /// 提交一个事务
        /// </summary>
        void Commit();

        /// <summary>
        /// 异步提交事务
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();


        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();

        /// <summary>
        /// 异步回滚事务
        /// </summary>
        /// <returns></returns>
        Task RollbackAsync();

        /// <summary>
        /// 内部执行一个数据库操作，事务内部开启
        /// </summary>
        /// <param name="Func"></param>
        /// <param name="Rollback"></param>
        void ExecuteTrans(Func<ICoreRepository, bool> Func, Action<Exception> Rollback = null);

        /// <summary>
        /// 内部执行一个数据库操作，事务内部开启，异步
        /// </summary>
        /// <param name="Func"></param>
        /// <param name="Rollback"></param>
        Task ExecuteTransAsync(Func<ICoreRepository, bool> Func, Action<Exception> Rollback = null);

        /// <summary>
        /// 更新一个实体
        /// </summary>
        /// <typeparam name="Tntity"></typeparam>
        /// <param name="tntity"></param>
        void Update<Tntity>(Tntity tntity) where Tntity : BaseEntity;

        /// <summary>
        /// 更新一个实体,异步
        /// </summary>
        /// <typeparam name="Tntity"></typeparam>
        /// <param name="tntity"></param>
        Task UpdateAsync<Tntity>(Tntity tntity) where Tntity : BaseEntity;


        /// <summary>
        /// 执行sql，返回表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        DataTable SqlQuery(string sql, SqlParameter[] parameters);

        /// <summary>
        /// 执行sql，返回表，异步
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<DataTable> SqlQueryAsync(string sql, SqlParameter[] parameters);

        /// <summary>
        /// 执行sql，返回实体
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
         IEnumerable<Tntity>  SqlQuery<Tntity>(string sql, SqlParameter[] parameters) where Tntity : BaseEntity, new();

        /// <summary>
        /// 执行sql，返回实体，异步
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        Task<IEnumerable<Tntity>> SqlQueryAsync<Tntity>(string sql, SqlParameter[] parameters) where Tntity : BaseEntity, new();


    }
}
