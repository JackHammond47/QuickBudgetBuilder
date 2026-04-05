namespace QuickBudgetBuilder.Models
{
    public class BudgetInput
    {
        public decimal MonthlyIncome { get; set; }
        public decimal Rent { get; set; }
        public string Lifestyle { get; set; }
        public string ZipCode { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public bool NeedsChildcare { get; set; }
    }
}
