using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;



namespace SWE3.Demo
{
    /// <summary>This class holds field metadata.</summary>
    public class Field
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="entity">Parent entity.</param>
        public Field(Entity entity)
        {
            Entity = entity;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the parent entity.</summary>
        public Entity Entity
        {
            get; set;
        }


        /// <summary>Gets the type field..</summary>
        public MemberInfo FieldMember
        {
            get; internal set;
        }


        /// <summary>Gets the column name in table.</summary>
        public string ColumnName
        {
            get; internal set;
        }


        /// <summary>Gets the field type</summary>
        public Type FieldType
        {
            get 
            {
                if(FieldMember is PropertyInfo) { return ((PropertyInfo) FieldMember).PropertyType; }

                throw new NotSupportedException("Member type not supported.");
            }
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the field value.</summary>
        /// <param name="obj">Object.</param>
        public object GetValue(object obj)
        {
            if(FieldMember is PropertyInfo) { return ((PropertyInfo) FieldMember).GetValue(obj); }

            throw new NotSupportedException("Member type not supported.");
        }


        /// <summary>Sets the field value.</summary>
        /// <param name="obj">Object.</param>
        /// <param name="value">Value.</param>
        public void SetValue(object obj, object value)
        {
            if(FieldMember is PropertyInfo) { ((PropertyInfo) FieldMember).SetValue(obj, value); return; }

            throw new NotSupportedException("Member type not supported.");
        }
    }
}
