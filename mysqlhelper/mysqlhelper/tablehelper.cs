// MySQLhelper : mySQLhelper
// Écrit par jfgratton (Jean-François Gratton), 2013.05.31 @ 11:30
// 
// tableHelper.cs : table-related methods

using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace JFG.mySQLhelper
{
    public partial class ConnectionHelper : IDisposable
    {
        /// <summary>
        /// Verifies if a table exists in a given database
        /// </summary>
        /// <param name="db"> The DB to check in</param>
        /// <param name="table"> The table to check for</param>
        /// <returns> True if the table exists, false otherwise</returns>
        /// <exception cref="MySqlException"></exception>
        public bool CheckIfTableExists(string db, string table)
        {
            bool bExists = true;
            object res = null;

            _mExceptionString = "";
            string cmdstr = "SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = @db AND table_name = @table";

            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdstr, _mconn);
                cmd.Parameters.AddWithValue("@db", db);
                cmd.Parameters.AddWithValue("@table", table);
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
                bExists = false;
            }

            return bExists;
        }

        /// <summary>
        /// Gets the number of rows in a table
        /// </summary>
        /// <param name="table"> Table to get the count</param>
        /// <param name="db"> Database containing the table</param>
        /// <returns> The number of rows (int) on success, -1 on failure (check _mExceptionString for the error message)</returns>
        /// <exception cref="MySqlException"></exception>
        public int GetRowCount(string table, string db = "")
        {
            int nRowCount = 0;
            string countString = "SELECT COUNT(*) FROM ";
            if (String.IsNullOrWhiteSpace(db) == false)
                countString += "@db.@table";
            else
                countString += "@table";

            try
            {
                MySqlCommand cmd = new MySqlCommand(countString, _mconn);
                if (String.IsNullOrWhiteSpace(db) == false)
                    cmd.Parameters.AddWithValue("@db", db);
                cmd.Parameters.AddWithValue("@table", table);
                MySqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow);
                while (rdr.Read())
                {
                    nRowCount = rdr.GetInt32(0);
                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (MySqlException mex)
            {
                _mExceptionString = mex.Message;
                nRowCount = -1;
            }
            return nRowCount;
        }

        //! Warning : this one is unprotectable
        /// <summary>
        /// Creates a given table with the columns given in parameters
        /// </summary>
        /// <param name="table"> Table name</param>
        /// <param name="tableCreateCmd"> Columns to create</param>
        /// <returns> True on success, false otherwise, and sets _mExceptionString</returns>
        /// <exception cref="MySqlException"></exception>
        public bool CreateTable(string table, string tableCreateCmd)
        {
            string cmdstr = "CREATE TABLE " + table + " " + tableCreateCmd;
            bool bOK = true;

            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdstr, _mconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (MySqlException mex)
            {
                _mExceptionString = mex.Message;
                bOK = false;
            }
            return bOK;
        }

        /// <summary>
        /// <para>Destroys the named table</para>
        /// <para>NO SQL INJECTION PROTECTION HERE</para>
        /// </summary>
        /// <param name="table"> Table to drop</param>
        /// <returns> True on success, false otherwise, and sets _mExceptionString</returns>
        /// <exception cref="MySqlException"></exception>
        public bool DropTable(string table)
        {
            string cmdstr = "DROP TABLE @table";
            bool bOK = true;

            try
            {
                MySqlCommand cmd = new MySqlCommand(cmdstr, _mconn);
                cmd.Parameters.AddWithValue("@table", table);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (MySqlException mex)
            {
                _mExceptionString = mex.Message;
                bOK = false;
            }
            return bOK;
        }

        //! Warning : this one is unprotectable
        /// <summary>
        /// <para>Executes a SQL statement (USE WITH CAUTION)</para>
        /// <para>NO SQL INJECTION PROTECTION HERE</para>
        /// </summary>
        /// <param name="command"> Command to execute</param>
        /// <returns> True on success, false otherwise, and sets _mExceptionString</returns>
        /// <exception cref="MySqlException"></exception>
        public bool Execute(string command)
        {
            bool bOK = true;

            try
            {
                MySqlCommand cmd = new MySqlCommand(command, _mconn);
                cmd.ExecuteNonQuery();
                cmd.Dispose();
            }
            catch (MySqlException mx)
            {
                _mExceptionString = mx.Message;
                bOK = false;
            }
            return bOK;
        }

        //! Warning : potentially unsecure
        /// <summary>
        /// Resets the auto-increment value of a selected field in a given table
        /// </summary>
        /// <param name="connectionString2"> Connection string (see below for the syntax)</param>
        /// <param name="database"> Database where the table to reset resides (string)</param>
        /// <param name="table"> Table where the reset is needed (string)</param>
        /// <param name="field"> Field to reset (string)</param>
        /// <returns> True if success, false otherwise (_mExceptionString might need to be checked)</returns>
        /// <exception cref="MySqlException"></exception>
        /// <para>NOTE:  connectionString2 should follow this syntax:</para>
        /// <para>"User ID=*USERNAME*;Password=*USER PASSWD*;Host=*SERVER*;Port=*DB PORT*;Database=*DB NAME*;</para>
        public bool ResetAutoIncrement(string connectionString2, string database, string table, string field)
        {
            //connectionString2 a la forme suivante:
            //"User ID=comptes;Password=comptes;Host=oslo;Port=3306;Database=comptabilite;"
            bool bOK = true;
            int nAutoIncrement = 1;
            string fqtn = database + "." + table;
            ConnectionHelper connHelp2 = new ConnectionHelper(connectionString2);
            connHelp2.OpenConnection();

            try
            {
                MySqlCommand cmd = new MySqlCommand("SELECT @field FROM @database.@table ORDER BY @field  ASC", _mconn);
                cmd.Parameters.AddWithValue("@field", field);
                cmd.Parameters.AddWithValue("@database", database);
                cmd.Parameters.AddWithValue("@table", table);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    int Inc = rdr.GetInt32(0);
                    if (Inc != nAutoIncrement)
                        connHelp2.Execute("UPDATE " + fqtn + " SET " + field + "=" + nAutoIncrement + " WHERE " + field + "=" + Inc);
                    ++nAutoIncrement;

                }
                rdr.Close();
                cmd.Dispose();
            }
            catch (MySqlException mex)
            {
                _mExceptionString = mex.Message;
                bOK = false;
            }
            return bOK && Execute("ALTER TABLE " + database + "." + table + " AUTO_INCREMENT=" + nAutoIncrement + 1);
        }
    }
}