using System;



namespace SWE3.Demo.Test
{
    /// <summary>This class represents a course in the school model.</summary>
    [entity(TableName = "COURSES")]
    public class Course
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets or sets the course ID.</summary>
        [pk]
        public string ID { get; set; }


        /// <summary>Gets or sets the active flag.</summary>
        [field(ColumnName = "HACTIVE", ColumnType = typeof(int))]
        public bool Active { get; set; }


        /// <summary>Gets or sets the course name.</summary>
        public string Name { get; set; }


        /// <summary>Gets or sets the course teacher.</summary>
        [fk(ColumnName = "KTEACHER")]
        public Teacher Teacher { get; set; }
    }
}
