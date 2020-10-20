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


        /// <summary>Gets the column database type.</summary>
        public Type ColumnType
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
        
        /// <summary>Returns a database column type equivalent for a field type value.</summary>
        /// <param name="value">Value.</param>
        /// <returns>Database type representation of the value.</returns>
        public object ToColumnType(object value)
        {
            if(FieldType == ColumnType) { return value; }

            if(value is bool)
            {
                if(ColumnType == typeof(int))   { return         (((bool) value) ? 1 : 0); }
                if(ColumnType == typeof(short)) { return (short) (((bool) value) ? 1 : 0); }
                if(ColumnType == typeof(long))  { return (long)  (((bool) value) ? 1 : 0); }
            }

            return value;
        }


        /// <summary>Returns a field type equivalent for a database column type value.</summary>
        /// <param name="value">Value.</param>
        /// <returns>Field type representation of the value.</returns>
        public object ToFieldType(object value)
        {
            if(FieldType == ColumnType) { return value; }

            if(FieldType == typeof(bool))
            {
                if(value is int)   { return ((int)   value != 0); }
                if(value is short) { return ((short) value != 0); }
                if(value is long)  { return ((long)  value != 0); }
            }

            return value;
        }


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
