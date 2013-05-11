using System.Configuration;
using System.Linq;

namespace Tournament
{
    public class Settings
    {
        public static string ConstantSalt
        {
            get
            {
                if (!ConfigurationManager.AppSettings.AllKeys.Contains("Tournament:ConstantSalt"))
                {
                    throw new ConfigurationErrorsException(string.Format("Configuration key {0} required; please check web.config is configured", "Tournament:ConstantSalt"));
                }
                return ConfigurationManager.AppSettings["Tournament:ConstantSalt"];
            }
        }
    }
}