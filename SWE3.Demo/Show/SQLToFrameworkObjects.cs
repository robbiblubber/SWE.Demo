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

            foreach(Teacher i in World.FromSQL<Teacher>("SELECT * FROM TEACHERS WHERE ID != 'T1'"))
            {
                Console.WriteLine(i.ID + ": [" + i.Name + "]");
            }
            Console.ReadLine();
        }
    }
}
