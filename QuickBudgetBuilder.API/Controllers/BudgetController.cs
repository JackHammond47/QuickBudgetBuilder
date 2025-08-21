using Microsoft.AspNetCore.Mvc;

namespace QuickBudgetBuilder.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        [HttpGet("calculate")]
        public IActionResult CalculateBudget([FromQuery] decimal income, [FromQuery] decimal rent)
        {
            if (income <= 0 || rent <= 0)
            {
                return BadRequest("Income and Rent must be greater than zero.");
            }

            var rentToIncomeRatio = rent / income;

            var sampleBudget = new
            {
                Income = income,
                Rent = rent,
                RentToIncomeRatio = Math.Round(rentToIncomeRatio, 2),
                Groceries = Math.Round(income * 0.1M, 2),
                Savings = Math.Round(income * 0.2M, 2),
                FunMoney = Math.Round(income * 0.05M, 2),
                Utilities = Math.Round(income * 0.08M, 2),
                EmergencyFund = Math.Round(income * 0.1M, 2)
            };

            return Ok(sampleBudget);
        }
    }
}
