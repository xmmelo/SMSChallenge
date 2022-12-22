using SMSChallenge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SMSChallenge.Managers
{
    public interface IConfigManager
    {
        void LoadConfig();
        SalaryConfig GetCountryConfig(string countryName);
    }

    public class ConfigManager : IConfigManager
    {

        private static string DIR_CONFIG_NAME = "//Config";
        public Dictionary<string, SalaryConfig> _countrySalaryConfig { get; set; }

        public ConfigManager()
        {
            LoadConfig(_countrySalaryConfig);
        }

        public void LoadConfig()
        {
            //Get config folder
            var dirName = Directory.GetCurrentDirectory();

            var configFiles = Directory.GetFiles(dirName + DIR_CONFIG_NAME);

            foreach (var file in configFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                GetConfig(fileName, file);
            }
        }

        public void LoadConfig(Dictionary<string, SalaryConfig> countrySalaryConfig)
        {
            //Get config folder
            var dirName = Directory.GetCurrentDirectory();

            var configFiles = Directory.GetFiles(dirName + DIR_CONFIG_NAME);

            foreach (var file in configFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                _countrySalaryConfig.TryAdd(fileName, GetConfig(fileName, file));
            }
        }

        public SalaryConfig GetCountryConfig(string countryName)
        {
            return _countrySalaryConfig.ContainsKey(countryName) ? _countrySalaryConfig[countryName] : throw new Exception($"No configuration for {countryName}");
        }

        private SalaryConfig GetConfig(string fileName, string filePath)
        {
            switch (fileName)
            {
                case "Portugal":
                    return Util.ParseToConfig<SalaryPortugalConfig>(filePath);
                case "France":
                    return Util.ParseToConfig<SalaryFranceConfig>(filePath);
                case "Germany":
                    return Util.ParseToConfig<SalaryGermanyConfig>(filePath);
                case "Italy":
                    return Util.ParseToConfig<SalaryItalyConfig>(filePath);
                default:
                    throw new Exception("Country config not implemented");
            }
        }
    }
}
