using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    /// <summary>This implementation shows how to create framework objects from native SQL.</summary>
    public static class SQLToFrameworkObjects
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Implements the demonstration.</summary>
        public static void Show()
        {
            World.Connection = new SQLiteConnection("Data Source=test.sqlite;Version=3;");
            World.Connection.Open();

            foreach(Course i in World.FromSQL<Course>("SELECT * FROM COURSES WHERE ID != 1"))
            {
                Console.WriteLine(i.ID + ": [" + i.Name + "]");
            }
            Console.ReadLine();
        }
    }
}
