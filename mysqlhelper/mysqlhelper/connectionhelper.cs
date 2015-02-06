// MySQLhelper : mySQLhelper
// Écrit par jfgratton (Jean-François Gratton), 2013.05.25 @ 07:51
// 
// connectionHelper.cs : methodes de gestion des connexions

using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace JFG.mySQLhelper
{
    /// <summary>
    /// ConnectionHelper : Connection management
    /// </summary>
    public partial class ConnectionHelper : IDisposable
    {
        private readonly string _mconnString;
        private bool _mDisconnect;
        private string _mExceptionString;
        private MySqlConnection _mconn;

        #region INIT

        /// <summary>
        /// First constructor : connection is created using a whole MySqlConnection class
        /// </summary>
        /// <param name="conn"> Connection handle (<see cref="MySqlConnection"/>)</param>
        /// <param name="bDisconnectAtEnd"> Should the connection be persistent ?</param>
        public ConnectionHelper(MySqlConnection conn, bool bDisconnectAtEnd = true)
        {
            _mconn = conn;
            _mconnString = _mExceptionString = "";
            _mDisconnect = bDisconnectAtEnd;
        }

        /// <summary>
        /// Second constructor : connection is created from a connection string
        /// </summary>
        /// <param name="connstr"> The connection string</param>
        /// <param name="bDisconnectAtEnd"> Should the connection be persistent ?</param>
        public ConnectionHelper(string connstr, bool bDisconnectAtEnd = true)
        {
            _mconn = null;
            _mconnString = connstr;
            _mDisconnect = bDisconnectAtEnd;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implements the IDispose interface
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposing) return;
            if (!_mDisconnect) return;
            _mconn.Close();
            _mconn.Dispose();
        }

        #endregion INIT

        /// <summary>
        /// Opens the connection to a remote MySQL server
        /// </summary>
        /// <returns> true if succesfull, false otherwise, and will set _mExceptionString</returns>
        /// <exception cref="MySqlException"></exception>
        public bool OpenConnection()
        {
            bool bOK = true;
            _mExceptionString = "";

            try
            {
                _mconn = new MySqlConnection(_mconnString);
                _mconn.Open();
            }

            catch (MySqlException mex)
            {
                bOK = false;
                _mExceptionString = mex.Message;
            }

            return bOK;
        }

        /// <summary>
        /// Closes the connection to a remote MySQL server
        /// </summary>
        /// <returns> True on success, false otherwise, and will set _mExceptionString</returns>
        /// <exception cref="MySqlException"></exception>
        public bool CloseConnection()
        {
            bool bOK = true;
            _mExceptionString = "";
            try
            {
                if (_mconn.State != ConnectionState.Closed)
                {
                    _mconn.Close();
                    _mDisconnect = true;
                }
            }
            catch (MySqlException mex)
            {
                _mExceptionString = mex.Message;
                bOK = false;
            }
            return bOK;
        }

        /// <summary>
        /// Returns the last recorded Exception message
        /// </summary>
        /// <returns> The Message member of the Exception class to be returned</returns>
        public string GetExceptionMessage()
        {
            return _mExceptionString;
        }

        /// <summary>
        /// Returns the <see cref="MySqlConnection"/> connection member from this class
        /// </summary>
        /// <returns> The connection handle</returns>
        public MySqlConnection GetConnection()
        {
            return _mconn;
        }
    }
}