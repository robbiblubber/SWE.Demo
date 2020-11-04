using System;
using System.Data.SQLite;

using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    /// <summary>This class shows how to create a single instance with an ID.</summary>
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

            Console.WriteLine(t.ID + " => " + t.Name);

            foreach(Class i in t.Classes)
            {
                Console.WriteLine("   " + i.Name);
            }

            Console.ReadLine();
        }
    }
}
