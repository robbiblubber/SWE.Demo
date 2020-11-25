
using System;
using System.Collections.Generic;
using System.Data.SQLite;

using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    public class UseCaching
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Implements the demonstration.</summary>
        public static void Show()
        {
            World.Connection = new SQLiteConnection("Data Source=test.sqlite;Version=3;");
            World.Connection.Open();
            
            World.CachingEnabled = false;

            Teacher t0 = World.GetObject<Teacher>("T0");
            Teacher t1 = World.GetObject<Teacher>("T0");
            Teacher t2 = World.GetObject<Teacher>("T0");

            Console.WriteLine("Caching disabled:");
            Console.WriteLine("t0 => " + t0.InstanceNumber.ToString());
            Console.WriteLine("t1 => " + t1.InstanceNumber.ToString());
            Console.WriteLine("t2 => " + t2.InstanceNumber.ToString());

            World.CachingEnabled = true;

            t0 = World.GetObject<Teacher>("T0");
            t1 = World.GetObject<Teacher>("T0");
            t2 = World.GetObject<Teacher>("T0");

            Console.WriteLine();
            Console.WriteLine("Caching enabled:");
            Console.WriteLine("t0 => " + t0.InstanceNumber.ToString());
            Console.WriteLine("t1 => " + t1.InstanceNumber.ToString());
            Console.WriteLine("t2 => " + t2.InstanceNumber.ToString());

            Console.ReadLine();
        }
    }
}
