using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;



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

            foreach(PropertyInfo i in t.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                if((ignoreAttribute) i.GetCustomAttribute(typeof(ignoreAttribute)) != null) continue;

                Field field = new Field(this);

                fieldAttribute fattr = (fieldAttribute) i.GetCustomAttribute(typeof(fieldAttribute));

                if(fattr != null)
                {
                    if(fattr is pkAttribute) 
                    { 
                        pks.Add(field);
                        field.IsPrimaryKey = true;
                    }

                    field.ColumnName = (fattr?.ColumnName ?? i.Name);
                    field.ColumnType = (fattr?.ColumnType ?? i.PropertyType);
                    
                    if(field.IsForeignKey = (fattr is fkAttribute))
                    {
                        field.IsExternal = typeof(IEnumerable).IsAssignableFrom(i.PropertyType);
                    }
                }
                else
                {
                    if((i.GetGetMethod() == null) || (!i.GetGetMethod().IsPublic)) continue;

                    field.ColumnName = i.Name;
                    field.ColumnType = i.PropertyType;
                }                
                field.FieldMember = i;

                fields.Add(field);
            }

            Fields = fields.ToArray();
            Externals = fields.Where(m => m.IsExternal).ToArray();
            Internals = fields.Where(m => (!m.IsExternal)).ToArray();

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


        /// <summary>Gets external fields.</summary>
        /// <remarks>External fields are referenced fields that do not belong to the underlying table.</remarks>
        public Field[] Externals
        {
            get; set;
        }


        /// <summary>Gets internal fields.</summary>
        public Field[] Internals
        {
            get; set;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public methods                                                                                                   //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Gets entity SQL.</summary>
        /// <returns>SQL string.</returns>
        public string GetSQL()
        {
            string query = "SELECT ";
            for(int i = 0; i < Internals.Length; i++)
            {
                if(i > 0) { query += ","; }
                query += Internals[i].ColumnName;
            }
            query += (" FROM " + TableName);

            return query;
        }
    }
}
