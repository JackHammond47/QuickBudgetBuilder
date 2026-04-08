namespace QuickBudgetBuilder.Services
{
public class HudSafmrService
{
    private readonly Dictionary<string, decimal> _rentByZip;
    private const decimal NationalAverage2BR = 1749.00m;

    public HudSafmrService(string csvPath)
    {
        _rentByZip = LoadCsv(csvPath);
    }

        private Dictionary<string, decimal> LoadCsv(string path)
        {
            var dict = new Dictionary<string, decimal>();

            foreach (var line in File.ReadLines(path).Skip(2))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                string cleanLine = System.Text.RegularExpressions.Regex.Replace(line, @"\""([^\""]*)\""", m => m.Value.Replace(",", ""));

                var cols = cleanLine.Split(',');

                if (cols.Length < 10) continue;

                string zip = cols[0].Trim().Trim('"').PadLeft(5, '0');
                string rentRaw = cols[9].Trim().Trim('"').Replace("$", "").Replace(",", "");

                if (decimal.TryParse(rentRaw, out decimal rent2BR))
                {
                    if (!dict.ContainsKey(zip) || rent2BR > dict[zip])
                        dict[zip] = rent2BR;
                }
            }

            foreach (var kvp in dict.Take(5))
                Console.WriteLine($"Loaded ZIP: '{kvp.Key}' length={kvp.Key.Length} => {kvp.Value}");
            Console.WriteLine($"Total ZIPs loaded: {dict.Count}");

            return dict;
        }

        public decimal GetMultiplier(string zipCode)
        {
            string cleanZip = zipCode.Trim().PadLeft(5, '0');
            Console.WriteLine($"Looking up ZIP: '{cleanZip}', length: {cleanZip.Length}");

            if (_rentByZip.TryGetValue(cleanZip, out decimal rent))
            {
                Console.WriteLine($"ZIP {cleanZip} found: rent={rent}, multiplier={rent / NationalAverage2BR}");
                return Math.Round(rent / NationalAverage2BR, 4);
            }

            Console.WriteLine($"ZIP {cleanZip} NOT found, using fallback 1.00");
            return 1.00m;
        }

        public decimal GetRent(string zipCode)
        {
            string cleanZip = zipCode.Trim().PadLeft(5, '0');
            if (_rentByZip.TryGetValue(cleanZip, out decimal rent))
                return rent;
            return 0m;
        }
    }
}
