using BincomMvcAssignment2.Models;
using Microsoft.AspNetCore.Mvc;


namespace BincomMvcAssignment2.Controllers
{
    public class TaxController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(decimal monthlyIncome)
        {
            var model = CalculateTax(monthlyIncome);
            return View("Result", model);
        }

        private TaxResult CalculateTax(decimal monthlyIncome)
        {
            decimal annualIncome = monthlyIncome * 12;
            decimal taxFreeAllowance = (0.2m * annualIncome) + 200000;
            decimal taxableIncome = annualIncome - taxFreeAllowance;

            if (taxableIncome <= 0)
                taxableIncome = 0;

            decimal tax = 0;
            decimal remaining = taxableIncome;

            // Apply tax brackets
            var brackets = new (decimal Limit, decimal Rate)[]
            {
                (300000, 0.07m),
                (300000, 0.11m),
                (500000, 0.15m),
                (500000, 0.19m),
                (1600000, 0.21m),
            };

            foreach (var bracket in brackets)
            {
                if (remaining > bracket.Limit)
                {
                    tax += bracket.Limit * bracket.Rate;
                    remaining -= bracket.Limit;
                }
                else
                {
                    tax += remaining * bracket.Rate;
                    remaining = 0;
                    break;
                }
            }

            // If income is still left, apply 24%
            if (remaining > 0)
            {
                tax += remaining * 0.24m;
            }

            return new TaxResult
            {
                MonthlyIncome = monthlyIncome,
                AnnualIncome = annualIncome,
                TaxableIncome = taxableIncome,
                TaxPayable = tax,
                NetAnnualIncome = annualIncome - tax
            };
        }
    }
}






