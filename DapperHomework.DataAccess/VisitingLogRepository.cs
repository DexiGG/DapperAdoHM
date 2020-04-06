using DapperHomework.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DapperHomework.DataAccess
{
    public class VisitingLogRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;
        public List<VisitingLog> GetRecordForThisDate(DateTime date)
        {
            using(var sql = new SqlConnection(connectionString))
            {
                return sql.Query<VisitingLog>("Select * from VisitingLog Where Date = @date", new { date }).ToList();
            }
        }

        public string GetGroupNameWithMaxVisiting()
        {
            using(var sql = new SqlConnection(connectionString))
            {
                return sql.Query<string>("Exec GetGroupName").FirstOrDefault();
            }
        }
    }
}