using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GeocodingService
{
    public class ReverseGeocodingService
    {
        public String ServiceUri { get; set; }
        public String Key { get; set; }

        public ReverseGeocodingService()
        {
            Key = "AIzaSyAqYbW3dgSuxFfjG4LI-qTEJ2v7iJFZwOo";
            ServiceUri = "https://maps.googleapis.com/maps/api/geocode/xml?" +
                "latlng={0},{1}&key={2}";
        }

        public string GetAddress(string latitude, string longitude)
        {
            string requestUri = string.Format(ServiceUri, latitude, longitude, Key);
            WebRequest request = WebRequest.Create(requestUri);
            request.Credentials = CredentialCache.DefaultCredentials;
            ((HttpWebRequest)request).UserAgent = ".NET Framework Example Client";
            WebResponse response = request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string responseXMLFromServer = reader.ReadToEnd();
            return ParseXML(responseXMLFromServer);
        }

        public String ParseXML(String XMLString)
        {
            String formattedAddress = "";
            var xmlElements = XElement.Parse(XMLString);

            var status = (from x in xmlElements.Descendants()
                          where x.Name == "status"
                          select x).FirstOrDefault();
            if (status.Value == "OK")
            {
                formattedAddress = (from x in xmlElements.Descendants()
                                    where x.Name == "formatted_address"
                                    select x).FirstOrDefault().Value;
            }
            else
            {
                formattedAddress = "Address don't find!";
            }
            return formattedAddress;
        }
    }
}
