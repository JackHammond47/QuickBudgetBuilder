namespace QuickBudgetBuilder.Models
{
    public class BudgetResult
    {
        public decimal Housing { get; set; }
        public decimal SuggestedHousing { get; set; }
        public bool HousingOverBudget { get; set; }
        public decimal Utilities { get; set; }
        public decimal Groceries { get; set; }
        public decimal EatingOut { get; set; }
        public decimal Transportation { get; set; }
        public decimal Health { get; set; }
        public decimal Insurance { get; set; }
        public decimal Savings { get; set; }
        public decimal Entertainment { get; set; }
        public decimal PersonalSpending { get; set; }
        public decimal Childcare { get; set; }
        public decimal Miscellaneous { get; set; }
        public decimal Remaining { get; set; }
        public decimal Investments { get; set; }
        public decimal Debt { get; set; }

        public List<string> Disclaimers { get; set; } = new();
    }
}