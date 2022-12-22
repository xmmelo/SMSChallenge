using Newtonsoft.Json;
using SMSChallenge.Models;
using System;
using System.IO;
using System.Linq;

public static class Util
{
    public static SalaryConfig ParseToConfig<T>(string filePath) where T : SalaryConfig
    {

        var salaryConfig = new SalaryConfig();

        using (StreamReader r = new StreamReader(filePath))
        {
            string json = r.ReadToEnd();
            salaryConfig = JsonConvert.DeserializeObject<T>(json);
        }

        return salaryConfig;
    }

    public static EnrichedRequest EnrinchRequest(SalaryRequest source, EnrichedRequest dest)
    {
        var sourceProps = typeof(SalaryRequest).GetProperties().Where(x => x.CanRead).ToList();
        var destProps = typeof(EnrichedRequest).GetProperties()
                .Where(x => x.CanWrite)
                .ToList();

        foreach (var sourceProp in sourceProps)
        {
            if (destProps.Any(x => x.Name == sourceProp.Name))
            {
                var p = destProps.First(x => x.Name == sourceProp.Name);
                if (p.CanWrite)
                {
                    p.SetValue(dest, sourceProp.GetValue(source, null), null);
                }
            }
        }

        dest.Age = DateTime.Now.Year - source.Birthday.Year;
        dest.Amount = source.HourlyRate * source.HoursWorked;

        return dest;
    }
}

