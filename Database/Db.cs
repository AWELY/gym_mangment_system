using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using gym_mangment_system.Properties;

namespace gym_mangment_system
{
    /// <summary>
    /// Central MSSQL Server access helper: resolves the connection string,
    /// opens connections, runs stored procedures and performs the database backup.
    /// </summary>
    public static class Db
    {
        /// <summary>
        /// Effective connection string: the user-editable override from Settings
        /// (set on the Settings page) when present, otherwise the "GymDb" entry in App.config.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                try
                {
                    string ovr = Settings.Default.SqlConnectionString;
                    if (!string.IsNullOrWhiteSpace(ovr))
                        return ovr.Trim();
                }
                catch { /* settings unavailable -> fall through to config */ }

                var cs = ConfigurationManager.ConnectionStrings["GymDb"];
                return cs != null ? cs.ConnectionString : string.Empty;
            }
        }

        public static SqlConnection GetOpenConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            conn.Open();
            return conn;
        }

        /// <summary>Creates a stored-procedure command on the given connection/transaction.</summary>
        public static SqlCommand Proc(string name, SqlConnection conn, SqlTransaction tx = null)
        {
            var cmd = new SqlCommand(name, conn) { CommandType = CommandType.StoredProcedure };
            if (tx != null) cmd.Transaction = tx;
            return cmd;
        }

        /// <summary>Adds a parameter, converting null/empty to DBNull.</summary>
        public static SqlParameter AddParam(this SqlCommand cmd, string name, object value)
        {
            return cmd.Parameters.AddWithValue(name, value ?? DBNull.Value);
        }

        /// <summary>Tries to open a connection. Returns false and a message on failure.</summary>
        public static bool TestConnection(out string error)
        {
            error = null;
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// Returns SQL Server's own default backup directory (read from the instance
        /// registry). The SQL Server service account always has write access to this
        /// folder, so it is a safe fallback when a user-chosen folder (e.g. the
        /// Desktop) is denied. Returns null if it cannot be determined.
        /// </summary>
        public static string GetDefaultBackupDirectory()
        {
            using (var conn = GetOpenConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText =
                    "DECLARE @dir NVARCHAR(512); " +
                    "EXEC master.dbo.xp_instance_regread " +
                    "N'HKEY_LOCAL_MACHINE', " +
                    "N'Software\\Microsoft\\MSSQLServer\\MSSQLServer', " +
                    "N'BackupDirectory', @dir OUTPUT; " +
                    "SELECT @dir;";
                object result = cmd.ExecuteScalar();
                return result == null || result == DBNull.Value
                    ? null
                    : result.ToString();
            }
        }

        /// <summary>
        /// Runs a full database backup into <paramref name="folder"/> via usp_BackupDatabase.
        /// The SQL Server service account must have write permission to that folder.
        /// Returns the full path of the created .bak file.
        /// </summary>
        public static string BackupTo(string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
                throw new ArgumentException("لم يتم تحديد مجلد النسخ الاحتياطي.", nameof(folder));

            using (var conn = GetOpenConnection())
            using (var cmd = Proc("dbo.usp_BackupDatabase", conn))
            {
                cmd.CommandTimeout = 120;
                cmd.AddParam("@BackupFolder", folder);
                var outParam = new SqlParameter("@BackupFile", SqlDbType.NVarChar, 500)
                {
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outParam);
                cmd.ExecuteNonQuery();
                return outParam.Value == null || outParam.Value == DBNull.Value
                    ? null
                    : outParam.Value.ToString();
            }
        }
    }
}
