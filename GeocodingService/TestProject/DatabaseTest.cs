using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeocodingService;
using System.Data;
using System.Data.SqlClient;

namespace GeocodingNTServiceTestProject
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void ConnectionOpenTest()
        {
            DatabaseOperations dbOperations = new DatabaseOperations();
            ConnectionState actualState = dbOperations.OpenDBConnection();
            ConnectionState expectedState = ConnectionState.Open;
            Assert.AreEqual(expectedState, actualState, "Connection openning successfull!");
        }
        
        [TestMethod]
        public void ConnectionCloseTest()
        {
            DatabaseOperations dbOperations = new DatabaseOperations();
            dbOperations.OpenDBConnection();
            ConnectionState actualState = dbOperations.CloseDBConnection();
            ConnectionState expectedState = ConnectionState.Closed;
            Assert.AreEqual(expectedState, actualState, "Connection closing successfull!");
        }

        [TestMethod]
        public void GetDataTest()
        {
            DatabaseOperations dbOperations = new DatabaseOperations();
            dbOperations.OpenDBConnection();
            dbOperations.DBCmd = new SqlCommand();
            dbOperations.DBCmd.CommandType = CommandType.StoredProcedure;
            dbOperations.DBCmd.Connection = dbOperations.DBConnection;
            dbOperations.DBCmd.CommandText = "GetDataFromKonum1";
            SqlDataReader rdr = dbOperations.DBCmd.ExecuteReader();
            String actualString = "";
            if (rdr.HasRows)
            {
                rdr.Read();
                actualString = rdr.GetValue(1).ToString();
            }
            String expectedString = "41.030896";
            Assert.AreEqual(expectedString, actualString);
        }

        [TestMethod]
        public void GetAllDataReturnValueTest()
        {
            DatabaseOperations db = new DatabaseOperations();
            String actualValue = db.GetLocationData("Konum1")["Longitude"][3];
            String expectedValue = "29.015479";
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod]
        public void UpsertAddresstest()
        {
            DatabaseOperations db = new DatabaseOperations();
            int actualCount = db.UpsertAddress("Konum1", 1, 
                "BALIKESİR");
            int expectedCount = 1;
            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
