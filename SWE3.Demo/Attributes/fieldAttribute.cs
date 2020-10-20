using System;



namespace SWE3.Demo
{
    /// <summary>This attribute marks a property as a field.</summary>
    public class fieldAttribute: Attribute
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public members                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Database column name.</summary>
        public string ColumnName = null;
    }
}
