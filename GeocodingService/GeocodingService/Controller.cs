using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeocodingService
{
    public class Controller
    {
        #region Properties
        public String TableName { get; set; }
        public DatabaseOperations DBOperations { get; set; }
        public ReverseGeocodingService ReverseGeoService { get; set; }
        /*
         * These data types specified as: 
         *  Dictionary <ColumnName, ColumnData>
         */
        public Dictionary<String, List<String>> LocationData { get; set; }
        public Dictionary<String, List<String>> AddressData { get; set; } 

        #endregion

        #region Constructors
        // Default Constructor
        public Controller() 
        {
            this.TableName = "Konum1";
            DBOperations = new DatabaseOperations();
            ReverseGeoService = new ReverseGeocodingService();
        }
        public Controller(String TableName)
        {
            this.TableName = TableName;
            DBOperations = new DatabaseOperations();
            ReverseGeoService = new ReverseGeocodingService();
        }
        #endregion

        #region Operator Method
        public int DoOperation(EventLog EventLog)
        {
            LocationData = GetDataFromTable(TableName);
            AddressData = GeocodingForLocationData(TableName, LocationData);
            int affectedRowCount = LoadAddressesToDatabase(TableName, AddressData);
            EventLog.WriteEntry(TableName + ": " + affectedRowCount + " row(s) affected");
            return affectedRowCount;
        }
        #endregion

        #region Inner Methods
        /// <summary>
        /// This methods will be private in the future
        /// </summary>
        public Dictionary<String, List<String>> GetDataFromTable(String TableName)
        {
            return DBOperations.GetLocationData(TableName);
        }

        public Dictionary<String, List<String>> GeocodingForLocationData(String TableName,
            Dictionary<String, List<String>> TableData)
        {
            AddressData = new Dictionary<string, List<string>>()
            {
                { "KonumID", new List<String>()},
                { "Adres", new List<String>()}
            };
            for (int i = 0; i < TableData["ID"].Count; i++)
            {
                AddressData["KonumID"].Add(TableData["ID"][i]);
                AddressData["Adres"].Add(ReverseGeoService.GetAddress(
                        TableData["Latitude"][i], TableData["Longitude"][i]));
            }
            return AddressData;
        }

        public int LoadAddressesToDatabase(String TableName, Dictionary<String, List<String>> AddressData)
        {
            int affectedRowCount = 0;
            for (int i = 0; i < AddressData["KonumID"].Count; i++)
            {
                affectedRowCount += DBOperations.UpsertAddress(
                    TableName,
                    Convert.ToInt32(AddressData["KonumID"][i]),
                    AddressData["Adres"][i]);
            }
            return affectedRowCount;
        }
        #endregion
        
    }
}
