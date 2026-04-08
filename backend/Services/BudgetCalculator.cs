using QuickBudgetBuilder.Models;

namespace QuickBudgetBuilder.Services
{
    public class BudgetCalculator
    {
        private readonly HudSafmrService _hudService;

        public BudgetCalculator(HudSafmrService hudService)
        {
            _hudService = hudService;
        }

        public BudgetResult Calculate(BudgetInput input)
        {
            decimal zipMultiplier = _hudService.GetMultiplier(input.ZipCode);

            decimal lifestyleMultiplier = input.Lifestyle?.ToLower() switch
            {
                "bare-bones" => 0.80m,
                "modest" => 1.00m,
                "comfortable" => 1.20m,
                "high-spending" => 1.50m,
                _ => 1.00m
            };

            int adults = input.Adults < 1 ? 1 : input.Adults;
            int children = input.Children < 0 ? 0 : input.Children;

            decimal housing = input.Rent;
            decimal suggestedHousing = input.MonthlyIncome * 0.25m;
            bool housingOverBudget = housing > suggestedHousing;

            decimal utilities = 224m * zipMultiplier;
            decimal groceries = (340m * adults + 250m * children) * zipMultiplier;
            decimal eatingOut = (250m * adults + 100m * children) * lifestyleMultiplier;
            decimal transportation = (298m * adults) * zipMultiplier;
            decimal health = adults == 1 ? 130.50m : (130.50m * adults * 0.9m);

            decimal insurance = adults + children > 1 ? 716m : 305m;

            decimal savings = input.MonthlyIncome * 0.15m;

            decimal entertainment = 297m * lifestyleMultiplier;
            decimal personalSpending = (210m * adults + 120m * children) * lifestyleMultiplier;

            decimal childcare = input.NeedsChildcare
                ? Math.Min(input.MonthlyIncome * 0.07m, 1230m * children)
                : 0m;

            decimal miscellaneous = input.MonthlyIncome * 0.05m;

            decimal used =
                housing +
                utilities +
                groceries +
                eatingOut +
                transportation +
                health +
                insurance +
                savings +
                entertainment +
                personalSpending +
                childcare +
                miscellaneous;

            decimal remaining = input.MonthlyIncome - used;

            decimal investments = remaining > 0 ? remaining : 0m;
            decimal debt = remaining < 0 ? Math.Abs(remaining) : 0m;


            return new BudgetResult
            {
                Housing = Math.Round(housing, 2),
                SuggestedHousing = Math.Round(suggestedHousing, 2),
                HousingOverBudget = housingOverBudget,
                Utilities = Math.Round(utilities, 2),
                Groceries = Math.Round(groceries, 2),
                EatingOut = Math.Round(eatingOut, 2),
                Transportation = Math.Round(transportation, 2),
                Health = Math.Round(health, 2),
                Insurance = Math.Round(insurance, 2),
                Savings = Math.Round(savings, 2),
                Entertainment = Math.Round(entertainment, 2),
                PersonalSpending = Math.Round(personalSpending, 2),
                Childcare = Math.Round(childcare, 2),
                Miscellaneous = Math.Round(miscellaneous, 2),
                Remaining = Math.Round(remaining, 2),
                Investments = Math.Round(investments, 2),
                Debt = Math.Round(debt, 2),
                Disclaimers = new List<string>
                {
                    "Housing is evaluated against a recommended 25% of monthly income, but your entered value is not modified.",
                    input.ZipCode != null && _hudService.GetRent(input.ZipCode) > 0
                    ? $"Based on data from the U.S. Department of Housing and Urban Development (HUD), the fair market rent for a 2-bedroom apartment for ZIP code {input.ZipCode} is ${_hudService.GetRent(input.ZipCode):N0}."
                    : "No HUD fair market rent data was found for your ZIP code.",
                    "Insurance estimates use national averages for car insurance and workplace health plans only.",
                    "Childcare estimates are simplified and may vary widely by age, location, and provider.",
                    "ZIP code adjustments use a simplified cost-of-living multiplier for selected areas.",
                    "Lifestyle selection adjusts discretionary spending categories such as entertainment and dining.",
                    "These values are planning estimates only and should be adjusted to match real household expenses."
                }
            };
        }
    }
}

/* 
 * Average spending per month in America
 * groceries= $340 per person
 * eating out = $250 per person
 * Utilities = $37(gas) + $129(power) + $58(water & public services) = $224 per month
 * Housing should be max 25% of income per month
 * Transportation = $179 (gas/fuel) + $38(other transportation) + $81(maintenance and repairs) = $298 per month
 * Health = $89 (medical services) + $41.50(medicine) = $130.50 per month
 * Insurance= varies wildly, avg is $191(car) and for workplace health insurance $114(individual)/$525(family plan) = $305(individual) or $7169(family)
 * Savings = 15% of income per month
 * Entertainment = $297 per month
 * Personal Spending = $64(personal care) + $146(apparel and services) = $210 per person
 * Childcare = usually max $1,230 for daycare, other options cheaper per child per month, decreases as children age, aim for 7% of income
 * Miscellanous Spending = ~5% of income per month
 * Debt = as much as possible (remaining amount, and cut out whatever possible)
 * Data from https://www.ramseysolutions.com/budgeting/budget-percentages and other sources
*/
