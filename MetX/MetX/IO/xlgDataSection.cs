using System.Configuration;

namespace MetX.IO
{
    /// <summary>Represents the xlg section in a configuration file</summary>
    // ReSharper disable UnusedMember.Global
    public class XlgDataSection : ConfigurationSection
    {
        /// <summary>Represents a collection of ProviderSettings objects</summary>
        [ConfigurationProperty("providers")]
        public ProviderSettingsCollection Providers => (ProviderSettingsCollection)base["providers"];

        /// <summary>The default provider to use</summary>
        [ConfigurationProperty("defaultProvider", DefaultValue = "SQLDataProvider")]
        public string DefaultProvider
        {
            get => (string)base["defaultProvider"];
            set => base["defaultProvider"] = value;
        }
        /// <summary>The default connection to use</summary>
        [ConfigurationProperty("connectionStringName", DefaultValue = "")]
        public string ConnectionStringName
        {
            get => (string)base["connectionStringName"];
            set => base["connectionStringName"] = value;
        }
    }
}
