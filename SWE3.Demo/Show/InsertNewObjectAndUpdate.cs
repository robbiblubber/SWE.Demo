using System;
using System.Data.SQLite;

using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    /// <summary>This class shows how to insert and update an object.</summary>
    public static class InsertNewObjectAndUpdate
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Implements the demonstration.</summary>
        public static void Show()
        {
            World.Connection = new SQLiteConnection("Data Source=test.sqlite;Version=3;");
            World.Connection.Open();

            Course c = new Course();
            c.ID = "X100";
            c.Name = "A New Hope";
            c.Teacher = World.GetObject<Teacher>("T0");
            World.Save(c);

            c.Teacher = World.GetObject<Teacher>("T2");
            World.Save(c);

            World.Delete(c);

            Console.ReadLine();
        }
    }
}
