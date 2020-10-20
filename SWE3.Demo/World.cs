using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE3.Demo
{
    /// <summary>This class grants access to the demo framework.</summary>
    public static class World
    {
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static members                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Entities.</summary>
        private static Dictionary<Type, Entity> _Entities = new Dictionary<Type, Entity>();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static properties                                                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the database connection used by the framework.</summary>
        public static IDbConnection Connection { get; set; }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets an entity descriptor for an object.</summary>
        /// <param name="o">Object.</param>
        /// <returns>Entity.</returns>
        internal static Entity _GetEntity(this object o)
        {
            Type t = null;
            if(o is Type)
            {
                t = (Type) o;
            }
            else { t = o.GetType(); }

            if(_Entities.ContainsKey(t)) return _Entities[t];

            Entity rval = new Entity(t);
            _Entities.Add(t, rval);
            return rval;
        }


        /// <summary>Creates an object from a database reader.</summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="re">Reader.</param>
        /// <returns>Object.</returns>
        internal static T _CreateObject<T>(IDataReader re)
        {
            T rval = (T) Activator.CreateInstance(typeof(T));

            foreach(Field i in rval._GetEntity().Fields)
            {
                i.SetValue(rval, re.GetValue(re.GetOrdinal(i.ColumnName)));
            }
            return rval;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>Returns an array of instances for an SQL query.</summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="sql">SQL query.</param>
        /// <returns>Instances.</returns>
        public static T[] FromSQL<T>(string sql)
        {
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = sql;
            IDataReader re = cmd.ExecuteReader();
            
            List<T> rval = new List<T>();
            while(re.Read())
            {
                rval.Add(_CreateObject<T>(re));
            }
            re.Close();
            cmd.Dispose();

            return rval.ToArray();
        }


        /// <summary>Creates an instance by its primary keys.</summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="pks">Primary keys.</param>
        /// <returns>Object.</returns>
        public static T GetObject<T>(params object[] pks)
        {
            Entity ent = typeof(T)._GetEntity();

            IDbCommand cmd = Connection.CreateCommand();

            string query = "SELECT ";
            for(int i = 0; i < ent.Fields.Length; i++) 
            {
                if(i > 0) { query += ","; }
                query += ent.Fields[i].ColumnName;
            }
            query += (" FROM " + ent.TableName + " WHERE ");

            for(int i = 0; i < ent.PrimaryKeys.Length; i++)
            {
                if(i > 0) { query += " AND "; }
                query += (":pkv_" + i.ToString());

                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = (":pkv_" + i.ToString());
                p.Value = pks[i];
                cmd.Parameters.Add(p);
            }
            cmd.CommandText = query;

            T rval;
            IDataReader re = cmd.ExecuteReader();
            if(re.Read())
            {
                rval = _CreateObject<T>(re);
            }
            else { throw new Exception("No data."); }

            re.Close();
            cmd.Dispose();

            return rval;
        }
    }
}
