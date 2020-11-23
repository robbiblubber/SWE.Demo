using System;
using System.Data.SQLite;

using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    /// <summary>This class shows how to add and remove objects from lists.</summary>
    public static class SaveObjectsWithLists
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Implements the demonstration.</summary>
        public static void Show()
        {
            World.Connection = new SQLiteConnection("Data Source=test.sqlite;Version=3;");
            World.Connection.Open();

            Teacher t = World.GetObject<Teacher>("T0");                         // 1:n
            Course c;
            t.Courses.Add(c = World.GetObject<Course>("X2"));
            World.Save(t);

            c.Teacher = World.GetObject<Teacher>("T2");
            
            c.Students.Add(World.GetObject<Student>("Z0"));                     // m:n
            c.Students.Add(World.GetObject<Student>("Z1"));
            World.Save(c);

            c.Students.Clear();
            World.Save(c);

            Console.ReadLine();
        }
    }
}
