using SMSChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSChallenge.Managers
{
    public interface ISalaryManager
    {
        int GetGrossSalary(int totalHours, int hourlyRate);
        SalaryModel GetSalaryDescription(SalaryRequest request);
    }

    public class SalaryManager : ISalaryManager
    {

        public Dictionary<string, Delegate> CountrySalaryCalculator { get; set; }
        private readonly IConfigManager _configManager;
        
        public SalaryManager(IConfigManager configManager)
        {
            _configManager = configManager ?? throw new ArgumentNullException(nameof(configManager));
            CountrySalaryCalculator =  _configManager.LoadConfig();
            
        }
        public int GetGrossSalary(int totalHours, int hourlyRate)
        {
            return CalculateGrossSalary(totalHours, hourlyRate);
        }

        public SalaryModel GetSalaryDescription(SalaryRequest request)
        {
            return CountrySalaryCalculator.ContainsKey(request.Location) ? (SalaryModel) CountrySalaryCalculator[request.Location].DynamicInvoke(Util.EnrinchRequest(request, new EnrichedRequest()), _configManager.GetCountryConfig(request.Location)) : throw new Exception("Country not configured");
        }

        private int CalculateGrossSalary(int hours, int salary)
        {
            return hours * salary;
        }

    }
}
