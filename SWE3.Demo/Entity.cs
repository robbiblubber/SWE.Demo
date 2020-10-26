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
            entityAttribute tattr = (entityAttribute) t.GetCustomAttribute(typeof(entityAttribute));
            if((tattr == null) || (string.IsNullOrWhiteSpace(tattr.TableName)))
            {
                TableName = t.Name;
            }
            else { TableName = tattr.TableName; }

            EntityType = t;
            List<Field> fields = new List<Field>();
            List<Field> pks = new List<Field>();

            foreach(PropertyInfo i in t.GetProperties())
            {
                Field field = new Field(this);

                fieldAttribute fattr = (fieldAttribute) i.GetCustomAttribute(typeof(fieldAttribute));

                if(fattr != null)
                {
                    if(fattr is pkAttribute) { pks.Add(field); }

                    field.ColumnName = (fattr?.ColumnName ?? i.Name);
                    field.ColumnType = (fattr?.ColumnType ?? i.PropertyType);
                }
                else 
                {                     
                    field.ColumnName = i.Name;
                    field.ColumnType = i.PropertyType;
                }                
                field.FieldMember = i;

                fields.Add(field);
            }

            Fields = fields.ToArray();
            PrimaryKeys = pks.ToArray();
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public properties                                                                                                //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets the primary keys.</summary>
        public Field[] PrimaryKeys
        {
            get; internal set;
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
