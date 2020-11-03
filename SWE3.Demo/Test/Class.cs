using System;



namespace SWE3.Demo.Test
{
    /// <summary>This class represents a class in the school model.</summary>
    [entity(TableName = "COURSES")]
    public class Class
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the class ID.</summary>
        [pk]
        public string ID { get; set; }


        /// <summary>Gets or sets the class name.</summary>
        public string Name { get; set; }
    }
}
