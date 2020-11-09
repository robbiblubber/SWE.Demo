﻿using System;



namespace SWE3.Demo
{
    /// <summary>This attribute marks a property as a foreign key field.</summary>
    public class fkAttribute: fieldAttribute
    {
        /// <summary>Specifies an assignement table for m:n relationships.</summary>
        /// <remarks>ColumnName dentotes the near foreign key of the assignment table, RemoteColumnName denotes the far key.</remarks>
        public string AssignmentTable = null;

        /// <summary>Specifies the far side foreign key in the assignment table.</summary>
        public string RemoteColumnName = null;
    }
}
