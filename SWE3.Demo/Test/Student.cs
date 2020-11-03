using System;



namespace SWE3.Demo.Test
{
    /// <summary>This is a student implementation (from School example).</summary>
    [entity(TableName = "STUDENTS")]
    public class Student: Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the student's grade.</summary>
        public int Grade { get; set; }
    }
}
