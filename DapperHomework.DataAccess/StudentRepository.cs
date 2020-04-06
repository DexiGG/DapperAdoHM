using System;
using Dapper;
using System.Configuration;
using System.Data.SqlClient;
using DapperHomework.Models;
using System.Collections.Generic;
using System.Linq;

namespace DapperHomework.DataAccess
{
    public class StudentRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["defaultConnection"].ConnectionString;

        public List<Student> GetStudentsList()
        {
            using(var sql = new SqlConnection(connectionString))
            {
                return sql.Query<Student>("Select * from Students").ToList();
            }
        }
        public List<Student> GetStudentsList(string column, object data)
        {
            using(var sql = new SqlConnection(connectionString))
            {
                return sql.Query<Student>($"Select * from Students Where {column} = @data", new { data }).ToList();
            }
        }
        public string GetStudentFullName(int studentId)
        {
            using(var sql = new SqlConnection(connectionString))
            {
                var student = sql.Query<Student>
                    ("Select * from Students Where Id = @studentId", new { studentId }).FirstOrDefault();
                return student.FirstName + " " + student.LastName + " " + student.MiddleName;
            }
        }
        public void Insert(Student student)
        {
            try
            {
                using (var sql = new SqlConnection(connectionString))
                {
                    var query = "INSERT INTO Students values (@FirstName, @LastName, @MiddleName, @GroupId)";
                    int result = sql.Execute(query, student);
                    if (result > 0)
                        Console.WriteLine("Успешное добавление");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.Write("Нажмите Enter чтобы продолжить, Escape чтобы выйти...");
        }

        public void Update(Student student)
        {
            try
            {
                using (var sql = new SqlConnection(connectionString))
                {
                    var query = "UPDATE Students " +
                        "SET FirstName = @FirstName, LastName = @LastName, MiddleName = @MiddleName, GroupId = @GroupId " +
                        "WHERE Id = @Id";
                    int result = sql.Execute(query, student);
                    if (result > 0)
                        Console.WriteLine("Успешное обновление");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.Write("Нажмите Enter чтобы продолжить, Escape чтобы выйти...");
        }

        public void Delete(Student student)
        {
            try
            {
                using (var sql = new SqlConnection(connectionString))
                {
                    var query = "Delete Students Where Id = @Id";
                    int result = sql.Execute(query, student);
                    if(result > 0)
                        Console.WriteLine("Успешное удаление");
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            Console.Write("Нажмите Enter чтобы продолжить, Escape чтобы выйти...");
        }
    }
}
