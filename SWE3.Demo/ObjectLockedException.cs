using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SWE3.Demo
{
    /// <summary>This exception is thrown when the requested object is already locked.</summary>
    public class ObjectLockedException: Exception
    {
        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="owner">Owner.</param>
        public ObjectLockedException(string owner)
        {
            OwnerKey = owner;
        }



        /// <summary>Gets the owner key that has locked the object.</summary>
        public string OwnerKey
        {
            get; private set;
        }
    }
}
