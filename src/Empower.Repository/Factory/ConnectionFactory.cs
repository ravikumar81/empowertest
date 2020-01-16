using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using Dapper;
using System.Text;
using System.Threading.Tasks;
using Empower.Entities;
using System.Reflection;

namespace Empower.Repository.Factory
{
    public class ConnectionFactory : IConnectionFactory
    {
        protected string _sqlConnection;

        public ConnectionFactory(string sqlConnection)
        {
            _sqlConnection = sqlConnection;
        }
        
        public List<T> GetData<T>(string sql)
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            List<T> data = null;
            

            using (var conn = factory.CreateConnection())
            {
                conn.ConnectionString = _sqlConnection;
                try
                {
                    data = conn.Query<T>(sql, commandTimeout: 30000).ToList();
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }
            }
            return data;

        }

        public List<T> ExecuteProcedure<T>(string procedureName ,object parameter = null)
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            List<T> data = null;


            using (var conn = factory.CreateConnection())
            {
                conn.ConnectionString = _sqlConnection;
                try
                {
                    if(Object.ReferenceEquals(parameter,null))
                    {
                        data = conn.Query<T>(procedureName, commandTimeout: 30000, commandType: CommandType.StoredProcedure).ToList();
                    }
                    else
                    {
                        data = conn.Query<T>(procedureName, parameter, commandTimeout: 30000, commandType: CommandType.StoredProcedure).ToList();
                    }
                    
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }

                }
            }
            return data;

        }
        public void AddMapper<T>(Dictionary<string, string> columnMaps)
        {
            
            var mapper = new Func<Type, string, PropertyInfo>((type, columnName) =>
            {
                if(columnMaps.ContainsKey(columnName))
                {
                    return type.GetProperty(columnMaps[columnName]);
                }else
                {
                    return type.GetProperty(columnName);
                }
            }

                );
            var map = new CustomPropertyTypeMap(
                typeof(T),
                (type, columnName) => mapper(type, columnName)
                );
            SqlMapper.SetTypeMap(typeof(T), map);
        }
    }
}
