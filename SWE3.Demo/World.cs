using System;
using System.Collections;
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
        /// <param name="objects">Cached objects.</param>
        /// <returns>Object.</returns>
        internal static T _CreateObject<T>(IDataReader re, ICollection<object> objects = null)
        {
            return (T) _CreateObject(typeof(T), re, objects);
        }


        /// <summary>Creates an object from a database reader.</summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="re">Reader.</param>
        /// <param name="objects">Cached objects.</param>
        /// <returns>Object.</returns>
        internal static object _CreateObject(Type t, IDataReader re, ICollection<object> objects = null)
        {
            object rval = _GetCachedObject(t, re, objects);

            if(rval == null)
            {
                if(objects == null) { objects = new List<object>(); }
                objects.Add(rval = Activator.CreateInstance(t));

                foreach(Field i in rval._GetEntity().PrimaryKeys)
                {
                    i.SetValue(rval, i.ToFieldType(re.GetValue(re.GetOrdinal(i.ColumnName))));
                }

                foreach(Field i in rval._GetEntity().Fields)
                {
                    if(!i.IsPrimaryKey)
                    {
                        i.SetValue(rval, i.ToFieldType(re.GetValue(re.GetOrdinal(i.IsExternal ? i.Entity.PrimaryKeys[0].ColumnName : i.ColumnName))), objects);
                    }
                }

                
            }

            return rval;
        }


        /// <summary>Creates an instance by its primary keys.</summary>
        /// <param name="t">Type.</param>
        /// <param name="pks">Primary keys.</param>
        /// <param name="objects">Cached obejcst.</param>
        /// <returns>Object.</returns>
        internal static object _CreateObject(Type t, IEnumerable<object> pks, ICollection<object> objects = null)
        {
            Entity ent = t._GetEntity();

            IDbCommand cmd = Connection.CreateCommand();

            string query = ent.GetSQL() + " WHERE ";

            for(int i = 0; i < ent.PrimaryKeys.Length; i++)
            {
                if(i > 0) { query += " AND "; }
                query += (ent.PrimaryKeys[i].ColumnName + " = " + (":pkv_" + i.ToString()));

                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = (":pkv_" + i.ToString());
                p.Value = pks.ElementAt(i);
                cmd.Parameters.Add(p);
            }
            cmd.CommandText = query;

            object rval;
            IDataReader re = cmd.ExecuteReader();
            if(re.Read())
            {
                rval = _CreateObject(t, re, objects);
            }
            else { throw new Exception("No data."); }

            re.Close();
            cmd.Dispose();

            return rval;
        }


        /// <summary>Fills a list </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">List.</param>
        /// <param name="re">Reader.</param>
        /// <param name="objects">Cached objects.</param>
        internal static void _FillList<T>(ICollection<T> list, IDataReader re, ICollection<object> objects = null)
        {
            _FillList(typeof(T), list, re, objects);
        }


        /// <summary>Fills a list.</summary>
        /// <param name="t">Type.</param>
        /// <param name="list">List.</param>
        /// <param name="re">Reader.</param>
        /// <param name="objects">Cached objects.</param>
        internal static void _FillList(Type t, object list, IDataReader re, ICollection<object> objects = null)
        {
            while(re.Read())
            {
                list.GetType().GetMethod("Add").Invoke(list, new object[] { _CreateObject(t, re, objects) });
            }
        }


        /// <summary>Fills a list.</summary>
        /// <param name="t">Type.</param>
        /// <param name="list">List.</param>
        /// <param name="sql">SQL query.</param>
        /// <param name="parameters">Parameters.</param>
        /// <param name="objects">Cached objects.</param>
        internal static void _FillList(Type t, object list, string sql, IEnumerable<Tuple<string, object>> parameters, ICollection<object> objects = null)
        {
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = sql;

            foreach(Tuple<string, object> i in parameters)
            {
                IDataParameter p = cmd.CreateParameter();
                p.ParameterName = i.Item1;
                p.Value = i.Item2;
                cmd.Parameters.Add(p);
            }

            IDataReader re = cmd.ExecuteReader();
            _FillList(t, list, re, objects);
            re.Close();
            re.Dispose();
            cmd.Dispose();
        }


        /// <summary>Searches the cached objects for an object and returns it..</summary>
        /// <param name="t">Type.</param>
        /// <param name="re">Reader.</param>
        /// <param name="objects">Cached objects.</param>
        /// <returns>Returns the cached object that matches the current reader or NULL if no such object has been found.</returns>
        private static object _GetCachedObject(Type t, IDataReader re, ICollection<object> objects)
        {
            if(objects == null) { return null; }
            foreach(object i in objects)
            {
                if(i.GetType() != t) continue;

                bool found = true;
                foreach(Field k in t._GetEntity().PrimaryKeys)
                {
                    if(!k.GetValue(i).Equals(k.ToFieldType(re.GetValue(re.GetOrdinal(k.ColumnName))))) { found = false; break; }
                }
                if(found) { return i; }
            }

            return null;
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
            _FillList<T>(rval, re);
            re.Close();
            re.Dispose();
            cmd.Dispose();

            return rval.ToArray();
        }


        /// <summary>Creates an instance by its primary keys.</summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="pks">Primary keys.</param>
        /// <returns>Object.</returns>
        public static T GetObject<T>(params object[] pks)
        {
            return (T) _CreateObject(typeof(T), pks);
        }
    }
}
