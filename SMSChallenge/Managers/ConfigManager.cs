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
        Dictionary<string, Delegate> LoadConfig();
        SalaryConfig GetCountryConfig(string countryName);
    }

    public class ConfigManager : IConfigManager
    {

        private static string DIR_CONFIG_NAME = "//Config";
        public Dictionary<string, SalaryConfig> _countrySalaryConfig = new Dictionary<string, SalaryConfig>();
        public ISalaryCalculatorManager _calculatorManager { get; set; }

        public ConfigManager(ISalaryCalculatorManager calculatorManager)
        {   
            _calculatorManager = calculatorManager ?? throw new ArgumentNullException(nameof(calculatorManager));
        }

        public Dictionary<string, Delegate> LoadConfig()
        {
            //Get config folder
            var dirName = Directory.GetCurrentDirectory();

            var configFiles = Directory.GetFiles(dirName + DIR_CONFIG_NAME);

            foreach (var file in configFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                _countrySalaryConfig.TryAdd(fileName, GetConfig(fileName, file));
            }

            return _calculatorManager.GetCalculator();

        }

        public void LoadConfig(Dictionary<string, Delegate> _countryCalculator)
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
                case nameof(Country.Portugal):
                    _calculatorManager.LoadOperation(fileName, SalaryCalculator.portugueseSalaryCalculator);
                    return Util.ParseToConfig<SalaryPortugalConfig>(filePath);
                case nameof(Country.Italy):
                    _calculatorManager.LoadOperation(fileName, SalaryCalculator.italySalaryCalculator);
                    return Util.ParseToConfig<SalaryItalyConfig>(filePath);
                case nameof(Country.Germany):
                    _calculatorManager.LoadOperation(fileName, SalaryCalculator.germanySalaryCalculator);
                    return Util.ParseToConfig<SalaryGermanyConfig>(filePath);
                case nameof(Country.France):
                    _calculatorManager.LoadOperation(fileName, SalaryCalculator.franceSalaryCalculator);
                    return Util.ParseToConfig<SalaryFranceConfig>(filePath);
                default:
                    throw new Exception("Country config not implemented");
            }
        }
    }
}
