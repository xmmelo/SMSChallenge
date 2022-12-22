using Newtonsoft.Json;
using SMSChallenge.Models;
using System.IO;

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
}

