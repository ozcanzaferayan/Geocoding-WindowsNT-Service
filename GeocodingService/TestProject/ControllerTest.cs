using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeocodingService;
using System.Collections.Generic;

namespace GeocodingNTServiceTestProject
{
    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void GetDataFromTableTest()
        {
            Controller c = new Controller();
            Dictionary<String, List<String>> tempData = c.GetDataFromTable("Konum1");
            int actualCount = tempData["ID"].Count;
            int expectedCount = 4;
            Assert.AreEqual(expectedCount, actualCount);
        }
        [TestMethod]
        public void GeocodingForLocationDataTest()
        {
            Controller c = new Controller();
            c.LocationData = c.GetDataFromTable("Konum1");
            c.AddressData = c.GeocodingForLocationData("Konum1", c.LocationData);
            int actualCount = c.AddressData["Adres"].Count;
            int expectedCount = 4;
            Assert.AreEqual(expectedCount, actualCount);
        }
        [TestMethod]
        public void LoadAddressesToDatabaseTest()
        {
            Controller c = new Controller();
            c.LocationData = c.GetDataFromTable("Konum1");
            c.AddressData = c.GeocodingForLocationData("Konum1", c.LocationData);
            int actualRowCount = c.LoadAddressesToDatabase("konum1", c.AddressData);
            int expectedRowCount = 4;
            Assert.AreEqual(expectedRowCount, actualRowCount);
        }
    }
}
