using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;



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

        /// <summary>Caches.</summary>
        private static Dictionary<Type, Cache> _Caches = new Dictionary<Type, Cache>();

        /// <summary>Empty cache.</summary>
        private static readonly Cache _NullCache = new NullCache();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static properties                                                                                         //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets or sets the database connection used by the framework.</summary>
        public static IDbConnection Connection { get; set; }


        /// <summary>Gets or sets if caching is enabled.</summary>
        public static bool CachingEnabled { get; set; } = true;


        /// <summary>Gets this world's owner key.</summary>
        public static string OwnerKey { get; } = new Random().Next(100000, 999999).ToString();



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // private static methods                                                                                           //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Gets the cache for a type.</summary>
        /// <param name="t">Type.</param>
        /// <returns>Cache.</returns>
        private static Cache _GetCache(Type t)
        {
            if(!CachingEnabled) { return _NullCache; }

            if(!_Caches.ContainsKey(t))
            {
                _Caches.Add(t, new Cache());
            }

            return _Caches[t];
        }


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

            if(!_Entities.ContainsKey(t))
            {
                _Entities.Add(t, new Entity(t));
            }

            return _Entities[t];
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

                foreach(Field i in t._GetEntity().PrimaryKeys)
                {
                    i.SetValue(rval, i.ToFieldType(re.GetValue(re.GetOrdinal(i.ColumnName))));
                }

                foreach(Field i in t._GetEntity().Fields)
                {
                    if(!i.IsPrimaryKey)
                    {
                        i.SetValue(rval, i.ToFieldType(re.GetValue(re.GetOrdinal(i.IsExternal ? i.Entity.PrimaryKeys[0].ColumnName : i.ColumnName))), objects);
                    }
                }

                _GetCache(t)[t._GetEntity().PrimaryKeys[0].GetValue(rval)] = rval;
            }

            return rval;
        }


        /// <summary>Creates an instance by its primary keys.</summary>
        /// <param name="t">Type.</param>
        /// <param name="pks">Primary keys.</param>
        /// <param name="objects">Cached objects.</param>
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

            object rval = null;
            IDataReader re = cmd.ExecuteReader();
            if(re.Read())
            {
                rval = _CreateObject(t, re, objects);
            }

            re.Close();
            cmd.Dispose();

            if(rval == null) { throw new Exception("No data."); }
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


        /// <summary>Searches the cached objects for an object and returns it.</summary>
        /// <param name="t">Type.</param>
        /// <param name="re">Reader.</param>
        /// <param name="objects">Cached objects.</param>
        /// <returns>Returns the cached object that matches the current reader or NULL if no such object has been found.</returns>
        private static object _GetCachedObject(Type t, IDataReader re, ICollection<object> objects)
        {
            if(objects != null)
            {
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
            }

            Field pk = t._GetEntity().PrimaryKeys[0];
            return _GetCache(t)[pk.ToFieldType(re.GetValue(re.GetOrdinal(pk.ColumnName)))]; ;
        }



        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // public static methods                                                                                            //
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        /// <summary>Clears the cache.</summary>
        public static void ClearCache()
        {
            _Caches = new Dictionary<Type, Cache>();
        }



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


        /// <summary>Saves an object.</summary>
        /// <param name="obj">Object.</param>
        public static void Save(object obj)
        {
            Entity ent = obj._GetEntity();

            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = ("INSERT INTO " + ent.TableName + " (");

            string update = "ON CONFLICT (";
            string insert = "";
            for(int i = 0; i < ent.PrimaryKeys.Length; i++)
            {
                if(i > 0) { update += ", "; }
                update += ent.PrimaryKeys[i].ColumnName;
            }
            update += ") DO UPDATE SET ";

            IDataParameter p;
            bool first = true;
            for(int i = 0; i < ent.Internals.Length; i++)
            {
                if(i > 0) { cmd.CommandText += ", "; insert += ", "; }
                cmd.CommandText += ent.Internals[i].ColumnName;

                insert += (":" + ent.Internals[i].ColumnName.ToLower() + "v");
                
                p = cmd.CreateParameter();
                p.ParameterName = (":" + ent.Internals[i].ColumnName.ToLower() + "v");
                p.Value = ent.Internals[i].ToColumnType(ent.Internals[i].GetValue(obj));
                cmd.Parameters.Add(p);

                if(!ent.Internals[i].IsPrimaryKey)
                {
                    if(first) { first = false; } else { update += ", "; }
                    update += (ent.Internals[i].ColumnName + " = " + (":" + ent.Internals[i].ColumnName.ToLower() + "w"));

                    p = cmd.CreateParameter();
                    p.ParameterName = (":" + ent.Internals[i].ColumnName.ToLower() + "w");
                    p.Value = ent.Internals[i].ToColumnType(ent.Internals[i].GetValue(obj));
                    cmd.Parameters.Add(p);
                }
            }
            cmd.CommandText += (") VALUES (" + insert + ") " + update);

            cmd.ExecuteNonQuery();
            cmd.Dispose();

            foreach(Field i in ent.Externals) { i.SaveReferences(obj); }

            _GetCache(obj.GetType())[ent.PrimaryKeys[0].GetValue(obj)] = obj;
        }


        /// <summary>Deletes an object.</summary>
        /// <param name="obj">Object.</param>
        public static void Delete(object obj)
        {
            Entity ent = obj._GetEntity();

            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = ("DELETE FROM " + ent.TableName + " WHERE ");
            IDataParameter p;

            for(int i = 0; i < ent.PrimaryKeys.Length; i++)
            {
                if(i > 0) { cmd.CommandText += " AND "; }
                cmd.CommandText += (ent.PrimaryKeys[i].ColumnName + " = " + (":" + ent.PrimaryKeys[i].ColumnName.ToLower() + "v"));

                p = cmd.CreateParameter();
                p.ParameterName = (":" + ent.PrimaryKeys[i].ColumnName.ToLower() + "v");
                p.Value = ent.PrimaryKeys[i].ToColumnType(ent.PrimaryKeys[i].GetValue(obj));
                cmd.Parameters.Add(p);
            }

            cmd.ExecuteNonQuery();
            cmd.Dispose();

            _GetCache(obj.GetType()).Delete(ent.PrimaryKeys[0].GetValue(obj));
        }


        /// <summary>Locks an object.</summary>
        /// <param name="obj">Object.</param>
        /// <exception cref="ObjectLockedException">Thrown when the object is locked.</exception>
        public static void Lock(object obj)
        {
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "INSERT INTO LOCKS (OWNER_KEY, TYPE_KEY, OBJECT_ID) VALUES (:own, :typ, :obj)";

            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":own";
            p.Value = OwnerKey;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":typ";
            p.Value = obj._GetEntity().TableName;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":obj";
            p.Value = obj._GetEntity().PrimaryKeys[0].GetValue(obj).ToString();
            cmd.Parameters.Add(p);

            bool success = true;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch(Exception) { success = false; }
            cmd.Dispose();

            if(!success)
            {
                string owner = IsLockedBy(obj);
                if(owner != OwnerKey) { throw new ObjectLockedException(owner); }
            }
        }


        /// <summary>Returns the owner key that locks an object.</summary>
        /// <param name="obj">Object.</param>
        /// <returns>Returns TRUE if the object is locked (by another owner), otherwise returns NULL.</returns>
        public static string IsLockedBy(object obj)
        {
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "SELECT OWNER_KEY FROM LOCKS WHERE TYPE_KEY = :typ AND OBJECT_ID = :obj";

            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":typ";
            p.Value = obj._GetEntity().TableName;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":obj";
            p.Value = obj._GetEntity().PrimaryKeys[0].GetValue(obj).ToString();
            cmd.Parameters.Add(p);

            string rval = (string) cmd.ExecuteScalar();
            cmd.Dispose();

            return rval;
        }


        /// <summary>Returns the if an object is locked.</summary>
        /// <param name="obj">Object.</param>
        /// <returns>Returns TRUE if the object is locked (by another owner), otherwise returns NULL.</returns>
        public static bool IsLocked(object obj)
        {
            return (IsLockedBy(obj) != OwnerKey);
        }


        /// <summary>Releases a lock on an object.</summary>
        /// <param name="obj">Object.</param>
        public static void ReleaseLock(object obj)
        {
            IDbCommand cmd = Connection.CreateCommand();
            cmd.CommandText = "DELETE FROM LOCKS WHERE OWNER_KEY = :own AND TYPE_KEY = :typ AND OBJECT_ID = :obj";

            IDataParameter p = cmd.CreateParameter();
            p.ParameterName = ":own";
            p.Value = OwnerKey;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":typ";
            p.Value = obj.GetType().FullName;
            cmd.Parameters.Add(p);

            p = cmd.CreateParameter();
            p.ParameterName = ":obj";
            p.Value = obj._GetEntity().PrimaryKeys[0].GetValue(obj).ToString();
            cmd.Parameters.Add(p);

            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }
    }
}
