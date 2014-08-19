using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace GeocodingService
{
    public class DatabaseOperations
    {
        #region Properties
        public SqlConnection DBConnection { get; set; }
        public string DBServerName { get; set; }
        public string DBIntegratedSecurity { get; set; }
        public string DBName { get; set; }
        public SqlCommand DBCmd { get; set; }
        public SqlDataReader DBReader { get; set; }
        public Dictionary<String, List<String>> DBData { get; set; }
        #endregion

        #region Constructor
        public DatabaseOperations()
        {
            DBServerName = @".\";
            DBIntegratedSecurity = "true";
            DBName = "GeocodingService";
            DBConnection = new SqlConnection
                (
                    @"Server= " + DBServerName + "; " +
                    "Integrated Security= " + DBIntegratedSecurity + "; " +
                    "Database= " + DBName
                );
        }
        #endregion

        #region Methods
        public ConnectionState OpenDBConnection()
        {
            if (DBConnection.State != ConnectionState.Open)
            {
                try
                {
                    DBConnection.Open();
                    return ConnectionState.Open;
                }
                catch (SqlException e)
                {
                    throw e;
                    return ConnectionState.Broken;
                }
            }
            else
            {
                return ConnectionState.Open;
            }
        }
        public ConnectionState CloseDBConnection()
        {
            if (DBConnection.State != ConnectionState.Closed)
            {
                try
                {
                    DBConnection.Close();
                    return ConnectionState.Closed;
                }
                catch (SqlException e)
                {
                    throw e;
                    return ConnectionState.Broken;
                }
            }
            else
            {
                return ConnectionState.Closed;
            }
        }
        public Dictionary<String, List<String>> GetLocationData(String TableName)
        {
            OpenDBConnection();
            DBData = new Dictionary<String, List<String>>() 
            { 
                {"ID", new List<String>()},
                {"Latitude", new List<String>()},
                {"Longitude", new List<String>()}
            };
            DBCmd = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "GetDataFrom" + TableName,
                Connection = DBConnection
            };
            try
            {
                DBReader = DBCmd.ExecuteReader();
                while (DBReader.Read())
                {
                    DBData["ID"].Add(DBReader.GetValue(0).ToString());
                    DBData["Latitude"].Add(DBReader.GetValue(1).ToString());
                    DBData["Longitude"].Add(DBReader.GetValue(2).ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                DBReader.Close();
            }
            
            return DBData;
        }
        public int UpsertAddress(String TableName, int KonumID ,String Address)
        {
            OpenDBConnection();
            DBCmd = new SqlCommand()
            {
                CommandType = CommandType.StoredProcedure,
                CommandText = "Upsert" + TableName + "Address",
                Connection = DBConnection
            };
            DBCmd.Parameters.Add("@KonumID", SqlDbType.Int);
            DBCmd.Parameters.Add("@Adres", SqlDbType.NVarChar);
            DBCmd.Parameters["@KonumID"].Value = KonumID;
            DBCmd.Parameters["@Adres"].Value = Address;
            int affectedRowCount = DBCmd.ExecuteNonQuery();
            return affectedRowCount;
        }
        #endregion
    }
}
