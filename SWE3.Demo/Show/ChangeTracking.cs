using System;
using System.Data.SQLite;

using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    /// <summary>This class shows change tracking.</summary>
    public static class ChangeTracking
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
            Console.WriteLine(t.FirstName);

            int uz = 0;

            Teacher u =  World.GetObject<Teacher>("T0");
            Console.WriteLine(t.FirstName);

            Console.WriteLine(World.GetHash(t));

            bool xxx = World.HasChanged(t);

            t.Salary += 20;
            bool xxy = World.HasChanged(t);

            World.Save(t);
            bool xxz = World.HasChanged(t);

            Console.ReadLine();
        }
    }
}
