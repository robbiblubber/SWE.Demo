using System;



namespace SWE3.Demo.Test
{
    /// <summary>This is a teacher implementation (from School example).</summary>
    [entity(TableName = "TEACHERS")]
    public class Teacher: Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets or sets the teacher's salary.</summary>
        public int Salary { get; set; }


        [field(ColumnName = "HDATE")]
        /// <summary>Gets or sets the teacher's hire date.</summary>
        public DateTime HireDate { get; set; }
    }
}
