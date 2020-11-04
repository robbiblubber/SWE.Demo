using System;



namespace SWE3.Demo.Test
{
    /// <summary>This class represents a class in the school model.</summary>
    [entity(TableName = "CLASSES")]
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


        /// <summary>Gets or sets the class teacher.</summary>
        [fk(ColumnName = "KTEACHER")]
        private Lazy<Teacher> _backTeacher { get; set; }


        /// <summary>Gets or sets the class teacher.</summary>
        [ignore]
        public Teacher Teacher
        {
            get { return _backTeacher.Value; }
            set { _backTeacher.Value = value; }
        }
    }
}
