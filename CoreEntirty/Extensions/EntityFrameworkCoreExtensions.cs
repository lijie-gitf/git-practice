using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CoreEntirty.Extensions
{
   public static class EntityFrameworkCoreExtensions
    {
        private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection connection, SqlParameter[] parameters=null, DbTransaction dbTransaction = null)
        {
            var conn = facade.GetDbConnection();
            connection = conn;
            if(connection.State== ConnectionState.Closed)
            {
                connection.Open();
            }
            var cmd = conn.CreateCommand();
            if (facade.IsSqlServer())
            {
                cmd.CommandText = sql;
                cmd.Transaction = facade.CurrentTransaction?.GetDbTransaction();
                cmd.Parameters.AddRange(parameters);
            }
            return cmd;
        }

        public static DataTable SqlQuery(this DatabaseFacade facade, string sql, SqlParameter[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);

            var reader = command.ExecuteReader();

            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }
        public static async Task<DataTable> SqlQueryAsync(this DatabaseFacade facade, string sql, SqlParameter[] parameters)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);

            var reader =await command.ExecuteReaderAsync();

            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }
        public static List<T> SqlQuery<T>(this DatabaseFacade facade, string sql, SqlParameter[] parameters) where T : BaseEntity,new()
        {
            var dt = SqlQuery(facade, sql, parameters);
            return dt.ToList<T>();
        }
        public static DataTable SqlQuery(this DatabaseFacade facade, string sql)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn);

            var reader = command.ExecuteReader();

            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }
        public static async Task<DataTable> SqlQueryAsync(this DatabaseFacade facade, string sql)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn);

            var reader = await command.ExecuteReaderAsync();

            var dt = new DataTable();
            dt.Load(reader);
            reader.Close();
            conn.Close();
            return dt;
        }
        public static async Task<List<T>> SqlQueryAsync<T>(this DatabaseFacade facade, string sql, SqlParameter[] parameters) where T : BaseEntity, new()
        {
            var dt =await SqlQueryAsync(facade, sql, parameters);
            return dt.ToList<T>();
        }
        public static int Update(this DatabaseFacade facade, string sql, SqlParameter[] parameters, DbTransaction dbTransaction = null)
        {
            var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
            return command.ExecuteNonQuery();
        }

        public static List<T> ToList<T>(this DataTable dt) where T : BaseEntity,new()
        {
            var propertyInfos = typeof(T).GetProperties();
            var list = new List<T>();
            foreach (DataRow row in dt.Rows)
            {
                var t = new T();
                foreach (PropertyInfo p in propertyInfos)
                {
                    if (dt.Columns.IndexOf(p.Name) != -1 && row[p.Name] != DBNull.Value)
                        p.SetValue(t, row[p.Name], null);
                }
                list.Add(t);
            }
            return list;
        }
    }
}
