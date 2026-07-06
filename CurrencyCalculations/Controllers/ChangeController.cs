using CurrencyCalculations.Models;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyCalculations.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChangeController : ControllerBase
    {
        [HttpPost]
        public IActionResult Calculate(ChangeRequest request)
        {
            if (request.AmountGiven <= 0 || request.ProductPrice <= 0)
            {
                return BadRequest("Values must be greater than zero.");
            }

            if (request.ProductPrice > request.AmountGiven)
            {
                return BadRequest("Product price cannot exceed amount given.");
            }

            int change = (int)Math.Round((request.AmountGiven - request.ProductPrice) * 100);

            var notes = new List<(string Name, int Value)>
            {
                ("£50",5000),
                ("£20",2000),
                ("£10",1000),
                ("£5",500),
                ("£2",200),
                ("£1",100),
                ("50p",50),
                ("20p",20),
                ("10p",10),
                ("5p",5),
                ("2p",2),
                ("1p",1)
            };

            List<DenominationResult> result = new();

            foreach (var note in notes)
            {
                int count = change / note.Value;

                if (count > 0)
                {
                    result.Add(new DenominationResult
                    {
                        Denomination = note.Name,
                        Count = count
                    });

                    change %= note.Value;
                }
            }

            return Ok(result);
        }
    }
}