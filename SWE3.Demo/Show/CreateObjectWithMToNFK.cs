using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    /// <summary>This class shows how to create a single instance with an ID.</summary>
    public class CreateObjectWithMToNFK
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Implements the demonstration.</summary>
        public static void Show()
        {
            World.Connection = new SQLiteConnection("Data Source=test.sqlite;Version=3;");
            World.Connection.Open();

            Course c = World.GetObject<Course>("X0");

            Console.WriteLine(c.ID + " => " + c.Name);
            foreach(Student i in c.Students)                                    // eager loading
            {
                Console.WriteLine("   " + i.Name + " (student)");
            }

            Console.WriteLine();
            Student s = World.GetObject<Student>("Z2");

            Console.WriteLine(s.ID + " => " + s.Name);
            foreach(Course i in s.Courses)                                     // lazy loading
            {
                Console.WriteLine("   " + i.Name + " (course)");
            }

            Console.ReadLine();
        }
    }
}
