using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE3.Demo.Test
{
    /// <summary>This is a person implementation (from School example).</summary>
    public abstract class Person
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets or sets the person ID.</summary>
        [pk]
        public string ID { get; set; }


        /// <summary>Gets or sets the person's name.</summary>
        public string Name { get; set; }


        /// <summary>Gets or sets the person's first name.</summary>
        public string FirstName { get; set; }


        /// <summary>Gets or sets the person's birth date.</summary>
        [field(ColumnName = "BDATE")]
        public DateTime BirthDate { get; set; }


        /// <summary>Gets or sets the person gender.</summary>
        public Gender Gender { get; set; }
    }



    /// <summary>This enumeration defines genders.</summary>
    public enum Gender: int
    {
        FEMALE = 0,
        MALE = 1
    }
}
