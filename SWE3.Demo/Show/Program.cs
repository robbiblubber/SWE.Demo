using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SWE3.Demo.Show
{
    /// <summary>This class contains the program entry point.</summary>
    class Program
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // entry point                                                                                                      //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Main entry point.</summary>
        /// <param name="args">Arguments</param>
        static void Main(string[] args)
        {
            /*FieldsFromEntity.Show();
            CreateInstanceByPK.Show();
            SQLToFrameworkObjects.Show();
            CreateObjectWithFK.Show();
            CreateObjectWithFKList.Show();
            CreateObjectWithMToNFK.Show();
            InsertNewObjectAndUpdate.Show();
            SaveObjectsWithLists.Show();
            UseCaching.Show();
            ObjectLocking.Show();
            */

            ChangeTracking.Show();
        }
    }
}
