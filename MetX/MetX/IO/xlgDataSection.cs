using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.Web.Configuration;

namespace MetX.IO
{
    /// <summary>Represents the xlg section in a configuration file</summary>
    // ReSharper disable UnusedMember.Global
    public class xlgDataSection : ConfigurationSection
    {
        /// <summary>Represents a collection of ProviderSettings objects</summary>
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers
        {
            get { return (ProviderSettingsCollection)base["providers"]; }
        }
        /// <summary>The default provider to use</summary>
        [ConfigurationProperty("defaultProvider", DefaultValue = "SQLDataProvider")]
        public string DefaultProvider
        {
            get { return (string)base["defaultProvider"]; }
            set { base["defaultProvider"] = value; }
        }
        /// <summary>The default connection to use</summary>
        [ConfigurationProperty("connectionStringName", DefaultValue = "")]
        public string ConnectionStringName
        {
            get { return (string)base["connectionStringName"]; }
            set { base["connectionStringName"] = value; }
        }
    }
}
