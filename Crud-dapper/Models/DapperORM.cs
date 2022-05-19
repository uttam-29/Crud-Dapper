using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

using System.Data.SqlClient;

namespace Crud_dapper.Models
{
    public static class DapperORM
    {
        private static string connectionString = @"Data Source=DESKTOP-SQ43BP0;Initial Catalog=Dapper-Crud;User ID=sa;Password=123";

        public static void Execute(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(procedureName, param, commandType: CommandType.StoredProcedure);
            }

        }

        //DapperORM.executeScalar<int>(,);
        public static T ExecuteScalar<T>(string procedureName, DynamicParameters param)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                return (T)Convert.ChangeType( conn.Execute(procedureName, param, commandType: CommandType.StoredProcedure),typeof(T));
            }

        }


        // DApperORM.ReturnList<EmployeeModel> <= IEnumerable<EmployeeModel>

        public static IEnumerable<T> ReturnList<T>(string procedureName, DynamicParameters param)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var a= conn.Query<T>(procedureName,param, commandType: CommandType.StoredProcedure);
                return a;
            }

        }


    }
}