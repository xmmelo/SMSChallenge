using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SMSChallenge.Models
{
    public class SalaryModel
    {
        public string Location { get; set; }
        public string GrossAmount { get; set; }
        public string IncomeTax { get; set; }
        public string UniversalSocialCharge { get; set; }
        public string Pension { get; set; }
        public string NetAmount { get; set; }
    }

    public class SalaryRequest
    {
        public string Location { get; set; }
        public DateTime Birthday { get; set; }
        public int HourlyRate { get; set; }
        public int HoursWorked { get; set; }

        public int Age { get; set; }
        public int Amount { get; set; }
    }

    public class SalaryConfig
    {
    }

    public class SalaryTaxRates : SalaryConfig
    {
        public List<TaxRate> TaxRates { get; set; }
        public double BasicPension { get; set; }
    }

    public class SalaryItalyConfig : SalaryTaxRates
    {
    }

    public class SalaryGermanyConfig : SalaryTaxRates
    {
    }
    public class SalaryFranceConfig : SalaryTaxRates
    {
        public List<UniversalSocialCharge> UniversalSocialCharges { get; set; }
    }
    public class SalaryPortugalConfig : SalaryConfig
    {
        public List<TaxBracket> TaxBrackets { get; set; }
        public List<UniversalSocialCharge> UniversalSocialCharges { get; set; }
    }
    public class TaxBracket
    {
        public int Rate { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
    }
    public class TaxRate
    {
        public int Rate { get; set; }
        public int? Value { get; set; }
        public int Order { get; set; }
    }
    public class UniversalSocialCharge
    {
        public double Rate { get; set; }
        public int Value { get; set; }
        public int Order { get; set; }
    }
}
