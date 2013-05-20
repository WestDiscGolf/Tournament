using System.Configuration;
using System.Linq;

namespace Tournament.Infrastructure
{
    public class Settings
    {
        private const string ConstantSaltKey = "Tournament:ConstantSalt";
        public static string ConstantSalt
        {
            get
            {
                if (!ConfigurationManager.AppSettings.AllKeys.Contains(ConstantSaltKey))
                {
                    throw new ConfigurationErrorsException(string.Format("Configuration key {0} required; please check web.config is configured", ConstantSaltKey));
                }
                return ConfigurationManager.AppSettings[ConstantSaltKey];
            }
        }

        private const string UseEmbeddedKey = "Tournament:UseEmbedded";
        public static bool UseEmbedded
        {
            get
            {
                if (!ConfigurationManager.AppSettings.AllKeys.Contains(UseEmbeddedKey))
                {
                    throw new ConfigurationErrorsException(string.Format("Configuration key {0} required; please check web.config is configured", UseEmbeddedKey));
                }

                string value = ConfigurationManager.AppSettings[UseEmbeddedKey];
                bool useEmbedded;
                if (bool.TryParse(value, out useEmbedded))
                {
                    return useEmbedded;
                }

                throw new ConfigurationErrorsException(string.Format("Configuration Key {0} has an incorrect value ('{1}'); please check web.config is configured", UseEmbeddedKey, value));
            }
        }
    }
}