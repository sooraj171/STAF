using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SATF;
using System;
using System.Collections.Generic;
using System.Data;

namespace STAF
{
    /// <summary>
    /// Simple helper for MS SQL Server operations. Reads connection strings from AppConfig (appsettings.json).
    /// Provides basic operations: open connection, verify, execute query, scalar, non-query and read results as list.
    /// </summary>
    public static class DbHelper
    {
        /// <summary>
        /// Gets a connection string from configuration. Default name is "DefaultConnection".
        /// </summary>
        public static string GetConnectionString(string name = "DefaultConnection")
        {
            IConfigurationRoot config = AppConfig.GetConfig();
            if (config == null) return null;

            // Try the helper first, then fallback to direct key
            string cs = config.GetConnectionString(name);
            if (!string.IsNullOrEmpty(cs)) return cs;

            return config[$"ConnectionStrings:{name}"];
        }

        /// <summary>
        /// Opens and returns a SqlConnection. Caller is responsible for disposing the connection.
        /// Throws InvalidOperationException when connection string cannot be found.
        /// </summary>
        public static SqlConnection OpenConnection(string name = "DefaultConnection")
        {
            string cs = GetConnectionString(name);
            if (string.IsNullOrEmpty(cs))
                throw new InvalidOperationException($"Connection string '{name}' not found in configuration.");

            var conn = new SqlConnection(cs);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// Verifies whether a connection can be opened. Returns true when connection opens successfully.
        /// </summary>
        public static bool VerifyConnection(string name = "DefaultConnection")
        {
            try
            {
                string cs = GetConnectionString(name);
                if (string.IsNullOrEmpty(cs)) return false;

                using var conn = new SqlConnection(cs);
                conn.Open();
                return conn.State == ConnectionState.Open;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Executes a query and returns results in a DataTable.
        /// </summary>
        public static DataTable ExecuteQuery(string sql, string connName = "DefaultConnection", Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentException("SQL must be provided", nameof(sql));

            string cs = GetConnectionString(connName);
            if (string.IsNullOrEmpty(cs)) throw new InvalidOperationException($"Connection string '{connName}' not found.");

            var dt = new DataTable();

            using var conn = new SqlConnection(cs);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.AddWithValue(p.Key.StartsWith("@") ? p.Key : "@" + p.Key, p.Value ?? DBNull.Value);
                }
            }

            using var da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            return dt;
        }

        /// <summary>
        /// Executes a non-query SQL (INSERT/UPDATE/DELETE) and returns affected rows count.
        /// </summary>
        public static int ExecuteNonQuery(string sql, string connName = "DefaultConnection", Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentException("SQL must be provided", nameof(sql));

            string cs = GetConnectionString(connName);
            if (string.IsNullOrEmpty(cs)) throw new InvalidOperationException($"Connection string '{connName}' not found.");

            using var conn = new SqlConnection(cs);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.AddWithValue(p.Key.StartsWith("@") ? p.Key : "@" + p.Key, p.Value ?? DBNull.Value);
                }
            }

            conn.Open();
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Executes scalar and returns the value converted to T (or default(T) on null).
        /// </summary>
        public static T ExecuteScalar<T>(string sql, string connName = "DefaultConnection", Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentException("SQL must be provided", nameof(sql));

            string cs = GetConnectionString(connName);
            if (string.IsNullOrEmpty(cs)) throw new InvalidOperationException($"Connection string '{connName}' not found.");

            using var conn = new SqlConnection(cs);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.AddWithValue(p.Key.StartsWith("@") ? p.Key : "@" + p.Key, p.Value ?? DBNull.Value);
                }
            }

            conn.Open();
            var result = cmd.ExecuteScalar();
            if (result == null || result == DBNull.Value) return default(T);
            return (T)Convert.ChangeType(result, typeof(T));
        }

        /// <summary>
        /// Executes reader and returns results as a list of dictionaries (column name -> value).
        /// Useful when you want dynamic access without DataTable.
        /// </summary>
        public static List<Dictionary<string, object>> GetDataAsList(string sql, string connName = "DefaultConnection", Dictionary<string, object> parameters = null)
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentException("SQL must be provided", nameof(sql));

            string cs = GetConnectionString(connName);
            if (string.IsNullOrEmpty(cs)) throw new InvalidOperationException($"Connection string '{connName}' not found.");

            var rows = new List<Dictionary<string, object>>();

            using var conn = new SqlConnection(cs);
            using var cmd = new SqlCommand(sql, conn);
            if (parameters != null)
            {
                foreach (var p in parameters)
                {
                    cmd.Parameters.AddWithValue(p.Key.StartsWith("@") ? p.Key : "@" + p.Key, p.Value ?? DBNull.Value);
                }
            }

            conn.Open();
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var dict = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    dict[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }
                rows.Add(dict);
            }

            return rows;
        }
    }
}
