using System;
using System.Collections.Generic;

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


        /// <summary>Gets or sets the course name.</summary>
        public string Name { get; set; }


        /// <summary>Gets or sets the course teacher.</summary>
        [fk(ColumnName = "KTEACHER")]
        public Teacher Teacher { get; set; }


        /// <summary>Gets or sets the students in this course.</summary>
        [fk(AssignmentTable = "STUDENT_COURSES", ColumnName = "KCOURSE", RemoteColumnName = "KSTUDENT")]
        public List<Student> Students { get; set; }
    }
}
