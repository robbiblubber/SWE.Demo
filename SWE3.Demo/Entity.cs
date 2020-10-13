using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace SWE3.Demo
{
    /// <summary>This class holds entity metadata.</summary>
    public sealed class Entity
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // constructors                                                                                                     //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Creates a new instance of this class.</summary>
        /// <param name="t">Type.</param>
        public Entity(Type t)
        {
            TableName = t.Name;
            EntityType = t;
            List<Field> fields = new List<Field>();

            foreach(PropertyInfo i in t.GetProperties())
            {
                Field field = new Field();

                fieldAttribute fattr = (fieldAttribute) i.GetCustomAttribute(typeof(fieldAttribute));

                if((fattr != null) && (fattr.ColumnName != null))
                {
                    field.ColumnName = fattr.ColumnName;
                }
                else { field.ColumnName = i.Name; }
                
                field.FieldMember = i;

                fields.Add(field);
            }

            Fields = fields.ToArray();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the primary keys.</summary>
        public Field[] PrimaryKeys
        {
            get
            {
                List<Field> rval = new List<Field>();
                foreach(Field i in Fields)
                {
                    if(i.FieldMember.GetCustomAttribute(typeof(pkAttribute)) != null) { rval.Add(i); }
                }

                return rval.ToArray();
            }
        }


        /// <summary>Gets or sets the entity type.</summary>
        public Type EntityType
        {
            get; set;
        }


        /// <summary>Gets the table name.</summary>
        public string TableName
        {
            get; private set;
        }


        /// <summary>Gets the entity fields.</summary>
        public Field[] Fields
        {
            get; private set;
        }
    }
}
