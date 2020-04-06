using DapperHomework.DataAccess;
using DapperHomework.Models;
using System;
using System.Collections.Generic;

namespace DapperHomework.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                int choice = 0;
                bool check = false;
                while (!check)
                {
                    System.Console.Clear();
                    System.Console.Write("\tБаза данных студентов\n" +
                        "1) Добавить студента\n" +
                        "2) Обновить студента\n" +
                        "3) Удалить студента\n\n" +
                        "4) Показать студентов по выбранной группе\n" +
                        "5) Показать посещения по выбранной дате\n" +
                        "6) Показать группу с наибольшим посещением\n" +
                        "Ваш выбор: ");
                    check = int.TryParse(System.Console.ReadLine(), out choice);
                    if (choice < 1 || choice > 6)
                        check = false;
                }

                System.Console.Clear();
                if (choice == 1)
                {
                    StudentRepository studentRepository = new StudentRepository();
                    Student student = new Student();
                    AddingFromConsole(student);

                    studentRepository.Insert(student);
                }
                else if(choice == 2)
                {
                    StudentRepository studentRepository = new StudentRepository();
                    Student student = FindTheRightStudent();
                    if (student != null)
                    {
                        AddingFromConsole(student);
                        studentRepository.Update(student);
                    }
                }
                else if (choice == 3)
                {
                    StudentRepository studentRepository = new StudentRepository();
                    Student student = FindTheRightStudent();

                    if (student != null)
                        studentRepository.Delete(student);
                }
                else if(choice == 4)
                {
                    System.Console.WriteLine("Введите название группы:");
                    string groupName = System.Console.ReadLine();
                    ShowTheStudents(groupName);
                }
                else if(choice == 5)
                {
                    DateTime date = DateTime.Now;
                    check = false;
                    while (!check)
                    {
                        System.Console.Clear();
                        System.Console.WriteLine("Введите дату:");
                        check = DateTime.TryParse(System.Console.ReadLine(), out date);
                    }

                    VisitingLogRepository visiting = new VisitingLogRepository();
                    var notes = visiting.GetRecordForThisDate(date);

                    ShowVisitingLog(notes);
                }
                else
                {
                    VisitingLogRepository visiting = new VisitingLogRepository();
                    string groupName = visiting.GetGroupNameWithMaxVisiting();

                    System.Console.Write("Группа: " + groupName +
                        "\n\nНажмите Enter чтобы продолжить, Escape чтобы выйти...");
                }
            } while (System.Console.ReadKey().Key != ConsoleKey.Escape);
        }

        #region Ввод данных на консоли
        private static void AddingFromConsole(Student student)
        {
            GroupRepository groupRepository = new GroupRepository();

            string[] properties = { "Имя: ", "Фамилия: ", "Отчество: ", "Номер группы: " };
            string[] data = new string[properties.Length];
            int groupId = 0;

            System.Console.WriteLine("Введите данные о студенте");
            for (int i = 0; i < properties.Length; i++)
            {
                System.Console.Write(properties[i]);
                data[i] = System.Console.ReadLine();
                if (properties[i] == "Номер группы: ")
                {
                    groupId = groupRepository.GetGroupId(data[i]);
                    if (groupId == 0)
                    {
                        i--;
                        System.Console.Write("Такой группы не существует, нажмите Enter чтобы ввести заново...");
                        System.Console.ReadLine();
                    }
                }
            }

            int counter = 0;
            student.FirstName = data[counter++];
            student.LastName = data[counter++];
            student.MiddleName = data[counter];
            student.GroupId = groupId;
        }
        #endregion

        #region Найти студента
        private static Student FindTheRightStudent()
        {
            StudentRepository studentRepository = new StudentRepository();
            GroupRepository groupRepository = new GroupRepository();

            System.Console.WriteLine("Введите фамилию студента:");
            string surname = System.Console.ReadLine();

            var students = studentRepository.GetStudentsList("LastName", surname);
            int studentPosition = 0;
            bool check = false;
            if (students.Count > 0)
            {
                for (int i = 0; i < students.Count; i++)
                {
                    System.Console.WriteLine($"{i + 1}) " +
                        $"{students[i].FirstName}   " +
                        $"{students[i].LastName}   " +
                        $"{students[i].MiddleName}   " +
                        $"{groupRepository.GetGroupName(students[i].GroupId)}");
                }
                
                while (!check)
                {
                    System.Console.Write("Выбор: ");
                    check = int.TryParse(System.Console.ReadLine(), out studentPosition);
                    if (studentPosition < 1 || studentPosition > students.Count)
                        check = false;
                }
                return students[studentPosition - 1];
            }
            else
            {
                System.Console.WriteLine("Не удалось найти ни одного студента по указанной фамилии\n" +
                "Нажмите Enter чтобы продолжить, Escape чтобы выйти...");
                return null;
            }
        }
        #endregion

        #region Показать студентов определенной группы
        private static void ShowTheStudents(string groupName)
        {
            StudentRepository studentRepository = new StudentRepository();
            GroupRepository groupRepository = new GroupRepository();
            int groupId = groupRepository.GetGroupId(groupName);

            var students = studentRepository.GetStudentsList("GroupId", groupId);

            if (students.Count > 0)
            {
                for (int i = 0; i < students.Count; i++)
                {
                    System.Console.WriteLine($"{i + 1}) " +
                        $"{students[i].FirstName}   " +
                        $"{students[i].LastName}   " +
                        $"{students[i].MiddleName}   " +
                        $"{groupRepository.GetGroupName(students[i].GroupId)}");
                }
            }
            else
                System.Console.WriteLine("Не удалось найти студентов по указанной группе");
            System.Console.Write("Нажмите Enter чтобы продолжить, Escape чтобы выйти...");
        }
        #endregion

        #region Показать посещения
        private static void ShowVisitingLog(List<VisitingLog> notes)
        {
            StudentRepository studentRepository = new StudentRepository();

            if (notes.Count > 0)
                for (int i = 0; i < notes.Count; i++)
                {
                    System.Console.Write($"{i + 1}) " +
                    $"{notes[i].Date.ToShortDateString()}   " +
                    $"{studentRepository.GetStudentFullName(notes[i].StudentId)}   ");
                    if (notes[i].Visit == true)
                        System.Console.WriteLine("Был(-а)");
                    else
                        System.Console.WriteLine("Не был(-а)");
                }
            else
                System.Console.WriteLine("По указанной дате не найдено записей.");
            System.Console.WriteLine("Нажмите Enter чтобы продолжить, Escape чтобы выйти...");
        }
        #endregion
    }
}
