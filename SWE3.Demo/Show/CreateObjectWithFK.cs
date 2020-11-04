using System;
using System.Data.SQLite;

using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    /// <summary>This class shows how to create a single instance with an ID.</summary>
    public static class CreateObjectWithFK
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Implements the demonstration.</summary>
        public static void Show()
        {
            World.Connection = new SQLiteConnection("Data Source=test.sqlite;Version=3;");
            World.Connection.Open();

            Class c = World.GetObject<Class>("C1");

            Console.Write(c.ID + " => " + c.Name);
            Console.WriteLine("(Teacher: " + c.Teacher.Name + ")");

            Console.ReadLine();
        }
    }
}
