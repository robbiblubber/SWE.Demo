using System;
using System.Data.SQLite;

using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    /// <summary>This class shows locking of an object.</summary>
    public static class ObjectLocking
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Implements the demonstration.</summary>
        public static void Show()
        {
            World.Connection = new SQLiteConnection("Data Source=test.sqlite;Version=3;");
            World.Connection.Open();

            Teacher t = World.GetObject<Teacher>("T2");
            
            World.Lock(t);
            t.Salary += 10;
            World.Save(t);
            World.ReleaseLock(t);

            Console.ReadLine();
        }
    }
}
