using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE3.Demo
{
    /// <summary>This attribute marks a property as a field.</summary>
    public class fieldAttribute: Attribute
    {
        /// <summary>Database column name.</summary>
        public string ColumnName = null;
    }
}
