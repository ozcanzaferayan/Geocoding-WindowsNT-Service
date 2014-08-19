using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace GeocodingService
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : System.Configuration.Install.Installer
    {
        protected override void OnBeforeInstall(IDictionary savedState) 
        {
            string parameter = "ReverseGeocodingSource\" \"ReverseGeocodingLogFile"; 
            Context.Parameters["assemblypath"] = "\"" + 
                Context.Parameters["assemblypath"] + "\" \"" + parameter + "\""; 
            base.OnBeforeInstall(savedState); 
        }
        public ProjectInstaller()
        {
            InitializeComponent();
        }
    }
}
