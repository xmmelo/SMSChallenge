using SMSChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class SalaryCalculator
{


    public static Func<EnrichedRequest, SalaryPortugalConfig, SalaryModel> portugueseSalaryCalculator = (request, config) => PortugalSalaryCalculator(config, request);
    public static Func<EnrichedRequest, SalaryGermanyConfig, SalaryModel> germanySalaryCalculator = (request, config) => GermanySalaryCalculator(config, request);
    public static Func<EnrichedRequest, SalaryItalyConfig, SalaryModel> italySalaryCalculator = (request, config) => ItalySalaryCalculator(config, request);
    public static Func<EnrichedRequest, SalaryFranceConfig, SalaryModel> franceSalaryCalculator= (request, config) => FranceSalaryCalculator(config, request);


    public static SalaryModel PortugalSalaryCalculator(SalaryPortugalConfig config, EnrichedRequest request)
    {
        var _liquid = (double)request.Amount;
        //Assign rate
        var rate = config.TaxBrackets.Where(x => x.MinValue >= _liquid && _liquid < x.MaxValue || _liquid >= x.MinValue && _liquid >= x.MaxValue).Select(x => x.Rate).FirstOrDefault();

        var taxed = _liquid * (rate / 100.0);
        _liquid -= taxed;
        //response.IncomeTax = taxed.ToString() + '€';

        //USC
        var usc = new double();
        if (request.Age >= 25)
        {
            //Apply usc
            usc = _liquid * (config.UniversalSocialCharges.FirstOrDefault().Rate / 100.0);
            _liquid -= usc;
            //response.USC = usc.ToString() + '€'

        }

        return new SalaryModel
        {
            NetAmount = _liquid.ToString(),
            GrossAmount = request.Amount.ToString(),
            IncomeTax = taxed.ToString(),
            UniversalSocialCharge = usc.ToString(),
            Location = request.Location,
            Pension = ""
        };
    }

    public static SalaryModel GermanySalaryCalculator(SalaryGermanyConfig config, EnrichedRequest request)
    {
        var increaseRate = request.Age >= 25 ? 5 : 0;
        var _liquid = (double)request.Amount;
        var totalTax = new double();

        var taxRates = config.TaxRates.OrderBy(x => x.Order);

        foreach (var taxrate in config.TaxRates)
        {
            var tax = (double) (taxrate.Rate + increaseRate);

            if (taxrate.Value != null && _liquid < taxrate.Value) break;

            if (taxrate.Value == null)
            {
                var taxed = _liquid * (tax / 100.0);
                totalTax += taxed;
                _liquid -= taxed;
                continue;
            }

            var tempTax = (double) taxrate.Value * (tax / 100.0);
            totalTax += tempTax;
            _liquid -= tempTax;
        }

        //Calculate pension
        var pension = _liquid * (config.Pension / 100.0);
        _liquid -= pension;

        return new SalaryModel
        {
            NetAmount = _liquid.ToString(),
            GrossAmount = request.Amount.ToString(),
            IncomeTax = totalTax.ToString(),
            UniversalSocialCharge = "",
            Pension = pension.ToString(),
            Location = request.Location

        };

    }

    public static SalaryModel ItalySalaryCalculator(SalaryItalyConfig config, EnrichedRequest request)
    {
        var _liquid = (double)request.Amount;
        var totalTax = new double();


        var pensionTax = request.Age >= 25 ? config.Pension * 2 : config.Pension;
        var taxRates = config.TaxRates.OrderBy(x => x.Order);

        foreach (var taxrate in config.TaxRates)
        {
            if (_liquid <= taxrate.Value)
            {
                var tempTaxed = _liquid * (taxrate.Rate / 100.0);
                _liquid -= tempTaxed;
                totalTax += tempTaxed;
                break;
            }

            var taxed = (double)taxrate.Value * (taxrate.Rate / 100.0);
            _liquid -= taxed;
            totalTax += taxed;
        }

        //Calculate pension
        var pension = _liquid * (config.Pension / 100.0);
        _liquid -= pension;

        return new SalaryModel
        {
            NetAmount = _liquid.ToString(),
            GrossAmount = request.Amount.ToString(),
            IncomeTax = totalTax.ToString(),
            UniversalSocialCharge = "",
            Location = request.Location,
            Pension = pension.ToString()

        };

    }

    public static SalaryModel FranceSalaryCalculator(SalaryFranceConfig config, EnrichedRequest request)
    {
        var _liquid = (double)request.Amount;
        var totalTax = new double();
        var uscTax = new double();


        var taxRates = config.TaxRates.OrderBy(x => x.Order);

        foreach (var taxrate in config.TaxRates)
        {

            if (taxrate.Value == null)
            {
                var tempTaxed = _liquid * (taxrate.Rate / 100.0);
                _liquid -= tempTaxed;
                totalTax += tempTaxed;
                break;
            }

            if (_liquid <= taxrate.Value)
            {
                var tempTaxed = _liquid * (taxrate.Rate / 100.0);
                _liquid -= tempTaxed;
                totalTax += tempTaxed;
                break;
            }

            var taxed = (double)taxrate.Value * (taxrate.Rate / 100.0);
            _liquid -= taxed;
            totalTax += taxed;
        }

        //USC
        var uscCharges = config.UniversalSocialCharges.OrderBy(x => x.Order);

        foreach (var usctax in uscCharges)
        {
            if (usctax.Value >= _liquid)
            {
                var tempTaxed = _liquid * (usctax.Rate / 100.0);
                _liquid -= tempTaxed;
                uscTax += uscTax;
                break;
            }

            var taxed = usctax.Value * (usctax.Rate / 100.0);
            _liquid -= taxed;
            uscTax += uscTax;

        }

        //Calculate pension
        var pension = _liquid * (config.Pension/ 100.0);
        _liquid -= pension;

        return new SalaryModel
        {
            NetAmount = _liquid.ToString(),
            GrossAmount = request.Amount.ToString(),
            IncomeTax = totalTax.ToString(),
            UniversalSocialCharge = uscTax.ToString(),
            Location = request.Location,
            Pension = pension.ToString()

        };

    }

}

