﻿using System;



namespace SWE3.Demo
{
    /// <summary>This attribute marks a class as an entity.</summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class entityAttribute: Attribute
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public members                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Table name.</summary>
        public string TableName;
    }
}
