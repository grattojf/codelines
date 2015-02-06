// MySQLhelper : mySQLhelper
// Écrit par jfgratton (Jean-François Gratton), 2013.05.31 @ 11:30
// 
// databaseHelper.cs : db-related methods

using System;
using System.Collections;
using MySql.Data.MySqlClient;

namespace JFG.mySQLhelper
{
    public partial class ConnectionHelper : IDisposable
    {
        /// <summary>
        /// Verifies if the database exists :)
        /// </summary>
        /// <param name="database"> Database name</param>
        /// <returns> True if the command succeeds, false if not
        /// or null if an error occured (check _mExceptionString if null)</returns>
        /// <exception cref="MySqlException"></exception>
        public bool? CheckIfDatabaseExists(string database)
        {
            bool? bExists = true;
            object res = null;
            _mExceptionString = "";

            try
            {
                MySqlCommand cmd =
                    new MySqlCommand("SELECT COUNT(*) FROM information_schema.schemata WHERE schema_name = @database", _mconn);
                cmd.Parameters.AddWithValue("@database", database);

                if ((res = cmd.ExecuteScalar()) == null)
                    bExists = false;
                else
                {
                    int r = Convert.ToInt32(res);
                    bExists = r != 0;
                }
            }
            catch (MySqlException mex)
            {
                _mExceptionString = mex.Message;
                bExists = null;
            }

            return bExists;
        }

        /// <summary>
        /// Creates a new database
        /// </summary>
        /// <param name="database"> The database name</param>
        /// <returns> True if the command succeeds, false otherwise and fills out the _mExceptionString property</returns>
        /// <exception cref="MySqlException"></exception>
        public bool CreateDB(string database)
        {
            string cmdstr = "CREATE DATABASE @database";
            bool bOK = true;

            _mExceptionString = "";
            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdstr, _mconn);
                cmd.Parameters.AddWithValue("@database", database);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException mex)
            {
                _mExceptionString = mex.Message;
                bOK = false;
            }
            return bOK;
        }

        /// <summary>
        /// Destroys an existing database
        /// </summary>
        /// <param name="database"> The database name</param>
        /// <returns> True if the command succeeds, false otherwise and fills out the _mExceptionString property</returns>
        /// <exception cref="MySqlException"></exception>
        public bool DropDB(string database)
        {
            string cmdstr = "DROP DATABASE @database";
            bool bOK = true;

            _mExceptionString = "";
            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdstr, _mconn);
                cmd.Parameters.AddWithValue("@database", database);
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException mex)
            {
                _mExceptionString = mex.Message;
                bOK = false;
            }
            return bOK;
        }

        /// <summary>
        /// Shows all tables from a given database
        /// </summary>
        /// <param name="alTableList"> The list of tables</param>
        /// <param name="database"> Database to fetch the schema from</param>
        /// <param name="like"> Filtering such as SHOW TABLES * AND table_name LIKE ...</param>
        /// <returns> True if the command succeeds, false otherwise and fills out the _mExceptionString property</returns>
        /// <exception cref="MySqlException"></exception>
        public bool ShowTables(ref ArrayList alTableList, string database, string like = "")
        {
            bool bOK = true;
            string cmdStr = "SELECT table_name FROM Information_Schema.Tables WHERE Table_Type = 'BASE TABLE' AND table_schema=@database";
            if (String.IsNullOrWhiteSpace(like) == false)
                cmdStr += " AND table_name LIKE @like";

            _mExceptionString = "";
            alTableList.Clear();

            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdStr, _mconn);
                cmd.Parameters.AddWithValue("@database", database);
                if (String.IsNullOrWhiteSpace(like) == false)
                    cmd.Parameters.AddWithValue("@like", like);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    alTableList.Add(rdr[0].ToString());
                }
                rdr.Close();
                cmd.Dispose();
            }

            catch (MySqlException mex)
            {
                _mExceptionString = mex.Message;
                bOK = false;
                alTableList.Clear();
            }

            return bOK;
        }
    }
}