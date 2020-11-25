﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace SWE3.Demo
{
    /// <summary>This class provides a cache.</summary>
    public class Cache
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // protected memvers                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Actual cache.</summary>
        protected Dictionary<object, object> _Cache;




        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        internal protected Cache()
        {
            _Cache = new Dictionary<object, object>();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets or sets a cached object.</summary>
        /// <param name="pk">Primary key.</param>
        /// <returns>Object.</returns>
        public virtual object this[object pk]
        {
            get 
            {
                if(_Cache.ContainsKey(pk)) return _Cache[pk];
                return null;
            }
            set
            {
                if(_Cache.ContainsKey(pk)) { _Cache[pk] = value; }
                _Cache.Add(pk, value);
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets if the cache contains an object.</summary>
        /// <param name="pk">Primary key.</param>
        /// <returns>Returns TRUE if the cache contains the object.</returns>
        public virtual bool Contains(object pk)
        {
            return _Cache.ContainsKey(pk);
        }


        /// <summary>Deletes an object from the cache.</summary>
        /// <param name="pk">Primary key.</param>
        public virtual void Delete(object pk)
        {
            if(_Cache.ContainsKey(pk)) { _Cache.Remove(pk); }
        }
    }
}
