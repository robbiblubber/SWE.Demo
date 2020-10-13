using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE3.Demo
{
    class Program
    {
        /// <summary>Main entry point.</summary>
        /// <param name="args">Arguments</param>
        static void Main(string[] args)
        {
            Entity enty = new Entity(typeof(TestPerson));

            Console.WriteLine("Entity: " + enty.EntityType.Name);
            Console.WriteLine("Table: " + enty.TableName);

            foreach(Field i in enty.Fields)
            {
                Console.Write(i.FieldMember.Name + " ==> " + i.ColumnName);
                if(enty.PrimaryKeys.Contains(i)) { Console.Write(" (PK)"); }
                Console.WriteLine();
            }

            Console.ReadLine();
        }
    }
}
