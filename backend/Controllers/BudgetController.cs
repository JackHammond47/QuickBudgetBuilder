using Microsoft.AspNetCore.Mvc;
using QuickBudgetBuilder.Models;
using QuickBudgetBuilder.Services;

namespace QuickBudgetBuilder.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BudgetController : ControllerBase
    {
        private readonly BudgetCalculator _calculator;

        public BudgetController(BudgetCalculator calculator)
        {
            _calculator = calculator;
        }

        [HttpPost("calculate")]
        public IActionResult CalculateBudget([FromBody] BudgetInput input)
        {
            var result = _calculator.Calculate(input);
            return Ok(result);
        }
    }
}
