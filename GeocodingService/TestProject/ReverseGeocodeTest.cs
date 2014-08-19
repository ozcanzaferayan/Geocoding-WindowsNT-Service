using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeocodingService;

namespace GeocodingNTServiceTestProject
{
    [TestClass]
    public class ReverseGeocodeTest
    {
        [TestMethod]
        public void GetAddressTest()
        {
            ReverseGeocodingService rgs = new ReverseGeocodingService(); //41.030896,28.873343
            String actualAddress = rgs.GetAddress("41.030896", "28.873343");
            String expectedAddress = "Gençosman Mh., Dülger Sokak 8-16, 34200 Güngören/Istanbul Province, Turkey";
            Assert.AreEqual(expectedAddress, actualAddress);
        }
    }
}
