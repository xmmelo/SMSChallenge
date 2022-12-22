using SMSChallenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


public class SalaryCalculator
{

    public double totalTax = 0;
    public double _liquid = 0;

    public SalaryModel PortugalSalaryCalculator(SalaryPortugalConfig config, SalaryRequest request)
    {
        _liquid = request.Amount;
        //Assign rate
        var rate = config.TaxBrackets.Where(x => x.MinValue >= _liquid && _liquid < x.MaxValue || _liquid >= x.MinValue && _liquid >= x.MaxValue).Select(x => x.Rate).FirstOrDefault();

        var taxed = _liquid * (rate / 100);
        _liquid -= taxed;
        //response.IncomeTax = taxed.ToString() + '€';

        //USC
        var usc = new double();
        if(request.Age >= 25)
        {
            //Apply usc
            usc = _liquid * config.UniversalSocialCharges.FirstOrDefault().Rate;
            _liquid -= usc;
            //response.USC = usc.ToString() + '€'

        }

        return new SalaryModel
        {
            NetAmount = _liquid.ToString(),
            GrossAmount = request.Amount.ToString(),
            IncomeTax = taxed.ToString(),
            UniversalSocialCharge = usc.ToString(),

        };
    }

    public SalaryModel GermanySalaryCalculator(SalaryGermanyConfig config, SalaryRequest request)
    {
        var increaseRate = request.Age >= 25 ? 5 : 0;
        _liquid = request.Amount;
        totalTax = 0;

        var taxRates = config.TaxRates.OrderBy(x => x.Order);

        foreach(var taxrate in config.TaxRates)
        {
            var tax = (taxrate.Rate + increaseRate);
            
            if (taxrate.Value != null && _liquid > taxrate.Value) break;

            if (taxrate.Value == null)
            {
                var taxed = _liquid * (tax / 100);
                totalTax += taxed;
                _liquid -= taxed;
                continue;
            }

            var tempTax = (double) taxrate.Value * (tax / 100);
            totalTax += tempTax;
            _liquid -= tempTax;
        }

        //Calculate pension
        var pension = _liquid * (config.BasicPension / 100);
        _liquid -= pension;

        return new SalaryModel
        {
            NetAmount = _liquid.ToString(),
            GrossAmount = request.Amount.ToString(),
            IncomeTax = totalTax.ToString(),
            UniversalSocialCharge = 0.ToString(),
            Pension = pension.ToString()

        };

    }

    public SalaryModel ItalySalaryCalculator(SalaryItalyConfig config, SalaryRequest request)
    {
        _liquid = request.Amount;
        totalTax = 0;

        
        var pensionTax = request.Age >= 25 ? config.BasicPension * 2 : config.BasicPension;
        var taxRates = config.TaxRates.OrderBy(x => x.Order);

        foreach (var taxrate in config.TaxRates)
        {
            if(_liquid <= taxrate.Value)
            {
                var tempTaxed = _liquid * (taxrate.Rate / 100);
                _liquid -= tempTaxed;
                totalTax += tempTaxed;
                break;
            }

            var taxed = (double) taxrate.Value * (taxrate.Rate / 100);            
            _liquid -= taxed;
            totalTax += taxed;
        }

        //Calculate pension
        var pension = _liquid * pensionTax;
        _liquid -= pension;

        return new SalaryModel
        {
            NetAmount = _liquid.ToString(),
            GrossAmount = request.Amount.ToString(),
            IncomeTax = totalTax.ToString(),
            UniversalSocialCharge = 0.ToString(),
            Pension = pension.ToString()

        };

    }

    public SalaryModel FranceSalaryCalculator(SalaryFranceConfig config, SalaryRequest request)
    {
        _liquid = request.Amount;
        totalTax = 0;
        var uscTax = new double();


        var taxRates = config.TaxRates.OrderBy(x => x.Order);

        foreach (var taxrate in config.TaxRates)
        {

            if (taxrate.Value == null)
            {
                var tempTaxed = _liquid * (taxrate.Rate / 100);
                _liquid -= tempTaxed;
                totalTax += tempTaxed;
                break;
            }

            if (_liquid <= taxrate.Value)
            {
                var tempTaxed = _liquid * (taxrate.Rate / 100);
                _liquid -= tempTaxed;
                totalTax += tempTaxed;
                break;
            }

            var taxed = (double)taxrate.Value * (taxrate.Rate / 100);
            _liquid -= taxed;
            totalTax += taxed;
        }

        //USC
        var uscCharges = config.UniversalSocialCharges.OrderBy(x => x.Order);

        foreach (var usctax in uscCharges)
        {
            if(usctax.Value >= _liquid)
            {
                var tempTaxed = _liquid * (usctax.Rate / 100);
                _liquid -= tempTaxed;
                uscTax += uscTax;
                break;
            }

            var taxed = usctax.Value * (usctax.Rate / 100);
            _liquid -= taxed;
            uscTax += uscTax;

        }

        //Calculate pension
        var pension = _liquid * config.BasicPension;
        _liquid -= pension;

        return new SalaryModel
        {
            NetAmount = _liquid.ToString(),
            GrossAmount = request.Amount.ToString(),
            IncomeTax = totalTax.ToString(),
            UniversalSocialCharge = uscTax.ToString(),
            Pension = pension.ToString()

        };

    }


}

