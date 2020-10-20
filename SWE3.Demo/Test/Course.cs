using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE3.Demo.Test
{
    /// <summary>This class represents a course in the school model.</summary>
    [tab(TableName = "COURSES")]
    public class Course
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets or sets the class ID.</summary>
        [pk]
        public string ID { get; set; }


        /// <summary>Gets or sets the active flag.</summary>
        [field(ColumnName = "HACTIVE", ColumnType = typeof(int))]
        public bool Active { get; set; }


        /// <summary>Course name.</summary>
        public string Name { get; set; }
    }
}
