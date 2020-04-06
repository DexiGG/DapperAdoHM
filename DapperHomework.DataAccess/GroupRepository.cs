using Dapper;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;

namespace DapperHomework.DataAccess
{
    public class GroupRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;

        public int GetGroupId(string groupName)
        {
            using (var sql = new SqlConnection(connectionString))
            {
                return sql.Query<int>($"Select id from Groups Where Name = @groupName", new { groupName }).FirstOrDefault();
            }
        }

        public string GetGroupName(int groupId)
        {
            using (var sql = new SqlConnection(connectionString))
            {
                return sql.Query<string>($"Select name from Groups Where id = @groupId", new { groupId }).FirstOrDefault();
            }
        }
    }
}
