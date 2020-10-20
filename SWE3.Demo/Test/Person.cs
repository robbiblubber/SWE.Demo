using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE3.Demo.Test
{
    /// <summary>This is a person implementation (from School example).</summary>
    public class Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets or sets the person ID.</summary>
        [pk]
        public string ID { get; set; }

        /// <summary>Gets or sets the person name.</summary>
        public string Name { get; set; }

        /// <summary>Gets or sets the person gender.</summary>
        [field(ColumnName = "IS_MALE")]
        public Gender Gender { get; set; }
    }



    /// <summary>This enumeration defiones genders.</summary>
    public enum Gender
    {
        FEMALE = 0,
        MALE = 1
    }
}
