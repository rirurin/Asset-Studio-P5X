using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetStudio.P5X
{
    public class Utility
    {
        public static string? FromBase64(string base64Data)
        {
            if (base64Data == "AAAAAA==") return null;
            else return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(base64Data));
        }
        public static Vector3? GetVector3(string base64Data)
        {
            string? strProper = FromBase64(base64Data);
            if (strProper == null) return null;
            string[] vecComps = strProper.Split(",");
            var x = (vecComps.Length > 0) ? float.Parse(vecComps[0]) : 0;
            var y = (vecComps.Length > 1) ? float.Parse(vecComps[1]) : 0;
            var z = (vecComps.Length > 2) ? float.Parse(vecComps[2]) : 0;
            return new Vector3(x, y, z);
        }
        public static Vector2? GetVector2(string base64Data)
        {
            string? strProper = FromBase64(base64Data);
            if (strProper == null) return null;
            string[] vecComps = strProper.Split(",");
            var x = (vecComps.Length > 0) ? float.Parse(vecComps[0]) : 0;
            var y = (vecComps.Length > 1) ? float.Parse(vecComps[1]) : 0;
            return new Vector2(x, y);
        }
        public static bool GetBool(long param) => (param != 0) ? true : false;
        public static T[]? Get1DArray<T>(string base64Data) where T : IConvertible
        {
            string? strProper = FromBase64(base64Data);
            if (strProper == null) return null;
            string[] strParts = strProper.Split(",");
            T[] values = new T[strParts.Length];
            for (int i = 0; i < strParts.Length; i++)
                // piss
                // https://stackoverflow.com/questions/8625/generic-type-conversion-from-string
                values[i] = (T)Convert.ChangeType(strParts[i], typeof(T));
            return values;
        }
        public static string[]? GetStringList(string strList)
        {
            if (strList.Length == 0) return null;
            return strList.Split(',');
        }
        public static Vector3[]? CreateVector3List(string vecList)
        {
            if (vecList.Length == 0) return null;
            var vectorComps = vecList.Split("|");
            Vector3[] patrolNodes = new Vector3[vectorComps.Length];
            for (int i = 0; i < patrolNodes.Length; i++)
            {
                string[] vectorAxes = vectorComps[i].Split(",");
                patrolNodes[i] = new Vector3(float.Parse(vectorAxes[0]), float.Parse(vectorAxes[1]), float.Parse(vectorAxes[2]));
            }
            return patrolNodes;
        }
        public static string[] GetColumnNames<TDBType>()
        {
            var fields = typeof(TDBType).GetFields();
            string[] columns = new string[fields.Length];
            for (int i = 0; i < fields.Length; i++)
                columns[i] = fields[i].Name;
            return columns;
        }
    }
    public class RunningConnection
    {
        public SqliteConnection Connection;
        public SqliteCommand Command;

        public RunningConnection(string dbName)
        {
            Connection = new SqliteConnection($"Data Source={dbName};Mode=ReadOnly");
            Connection.Open();
            Command = Connection.CreateCommand();
        }
        public void Close() => Connection.Close();
        public List<TReturnType> GetEntries<TReturnType>(Condition? filter = null)
        {
            var query = SelectQuery.CreateQuery(typeof(TReturnType), null, filter);
            Command.CommandText = query.ToString();
            List<TReturnType> triggers = new List<TReturnType>();
            using (var reader = Command.ExecuteReader())
            {
                while (reader.Read())
                {
                    object[] rowResult = new object[query.GetColumnCount()];
                    reader.GetValues(rowResult);
                    triggers.Add((TReturnType)typeof(TReturnType).GetConstructors().First().Invoke(new object[] { rowResult }));
                }
            }
            return triggers;
        }
        public TReturnType GetEntry<TReturnType>(Condition? filter = null) => GetEntries<TReturnType>(filter).First();
    }
    public class ConnectionManager
    {
        private string Folder;
        public Dictionary<string, RunningConnection> OpenDbConnections;

        public ConnectionManager(string folder)
        {
            Folder = folder;
            OpenDbConnections = new Dictionary<string, RunningConnection>();
        }
        public RunningConnection GetDatabase(string dbName)
        {
            if (OpenDbConnections.TryGetValue(dbName, out RunningConnection connection)) return connection;
            // if not found, make a new connection
            var newConn = new RunningConnection($"{Folder}\\{dbName}.bytes");
            OpenDbConnections.Add(dbName, newConn);
            return newConn;
        }
        public void CloseConnections()
        {
            foreach (var conn in OpenDbConnections.Values)
                conn.Close();
        }
    }
}
