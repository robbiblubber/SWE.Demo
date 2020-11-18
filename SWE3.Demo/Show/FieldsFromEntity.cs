using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SWE3.Demo;
using SWE3.Demo.Test;



namespace SWE3.Demo.Show
{
    /// <summary>This implementation shows how to read data from an entity.</summary>
    public static class FieldsFromEntity
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Implements the demonstration.</summary>
        public static void Show()
        {
            Entity ent = typeof(Teacher)._GetEntity();

            foreach(Field i in ent.Fields)
            {
                Console.Write(i.FieldMember.Name + " => " + ent.TableName + "." + i.ColumnName);
                if(i.IsPrimaryKey) { Console.Write(" (pk)"); }
                if(i.IsForeignKey) { Console.Write(" (fk)"); }
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
