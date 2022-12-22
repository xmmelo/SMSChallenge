using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSChallenge.Managers
{
    public interface ISalaryCalculatorManager
    {
        void LoadOperation(string country, Delegate function);

        Dictionary<string, Delegate> GetCalculator();
    }
    public class SalaryCalculatorManager : ISalaryCalculatorManager
    {
        private readonly Dictionary<string, Delegate> _salaryCalculator = new Dictionary<string, Delegate>();
        public SalaryCalculatorManager()
        {
        }

        public void LoadOperation(string country, Delegate function)
        {
            _salaryCalculator.TryAdd(country, function);
        }

        public Dictionary<string, Delegate> GetCalculator()
        {
            return _salaryCalculator;
        }
    }
}
