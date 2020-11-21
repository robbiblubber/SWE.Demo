using System;
using System.Data.SQLite;

using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    /// <summary>This class shows how to create an instance with lists from foreign keys.</summary>
    public static class CreateObjectWithFKList
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Implements the demonstration.</summary>
        public static void Show()
        {
            World.Connection = new SQLiteConnection("Data Source=test.sqlite;Version=3;");
            World.Connection.Open();

            Teacher t = World.GetObject<Teacher>("T0");

            Console.WriteLine(t.ID + ": [" + t.Name + "]");
            foreach(Class i in t.Classes)                                       // with eager loading
            {
                Console.WriteLine("   " + i.Name + " (class)");
            }
            
            foreach(Course i in t.Courses)                                      // with lazy loading
            {
                Console.WriteLine("   " + i.Name + " (course)");
            }
            Console.ReadLine();
        }
    }
}
