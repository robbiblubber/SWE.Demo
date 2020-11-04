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
        /// <returns>Object.</returns>
        internal static T _CreateObject<T>(IDataReader re)
        {
            return (T) _CreateObject(typeof(T), re);
        }


        /// <summary>Creates an object from a database reader.</summary>
        /// <typeparam name="T">Type.</typeparam>
        /// <param name="re">Reader.</param>
        /// <returns>Object.</returns>
        internal static object _CreateObject(Type t, IDataReader re)
        {
            object rval = Activator.CreateInstance(t);

            foreach(Field i in rval._GetEntity().Fields)
            {
                i.SetValue(rval, i.ToFieldType(re.GetValue(re.GetOrdinal(i.IsExternal ? i.Entity.PrimaryKeys[0].ColumnName : i.ColumnName))));
            }

            return rval;
        }


        /// <summary>Fills a list </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">List.</param>
        /// <param name="re">Reader.</param>
        internal static void _FillList<T>(ICollection<T> list, IDataReader re)
        {
            _FillList(typeof(T), list, re);
        }


        /// <summary>Fills a list.</summary>
        /// <param name="t">Type.</param>
        /// <param name="list">List.</param>
        /// <param name="re">Reader.</param>
        internal static void _FillList(Type t, object list, IDataReader re)
        {
            while(re.Read())
            {
                list.GetType().GetMethod("Add").Invoke(list, new object[] { _CreateObject(t, re) });
            }
        }


        /// <summary>Fills a list.</summary>
        /// <param name="t">Type.</param>
        /// <param name="list">List.</param>
        /// <param name="sql">SQL query.</param>
        /// <param name="parameters">Parameters.</param>
        internal static void _FillList(Type t, object list, string sql, params Tuple<string, object>[] parameters)
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
            _FillList(t, list, re);
            re.Close();
            re.Dispose();
            cmd.Dispose();
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
            return (T) GetObject(typeof(T), pks);
        }


        /// <summary>Creates an instance by its primary keys.</summary>
        /// <param name="t">Type.</param>
        /// <param name="pks">Primary keys.</param>
        /// <returns>Object.</returns>
        public static object GetObject(Type t, params object[] pks)
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
                p.Value = pks[i];
                cmd.Parameters.Add(p);
            }
            cmd.CommandText = query;

            object rval;
            IDataReader re = cmd.ExecuteReader();
            if(re.Read())
            {
                rval = _CreateObject(t, re);
            }
            else { throw new Exception("No data."); }

            re.Close();
            cmd.Dispose();

            return rval;
        }
    }
}
