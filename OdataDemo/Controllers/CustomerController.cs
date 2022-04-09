using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;

namespace OdataDemo.Controllers
{
    public class CustomerController : ODataController
    {
        private readonly OdataOrderContext odataOrderContext;

        public CustomerController(OdataOrderContext odataOrderContext)
        {
            this.odataOrderContext = odataOrderContext;
        }

        [EnableQuery]
        public IActionResult Get() => Ok(odataOrderContext.Customers);

        [HttpPost]
        public async Task<IActionResult> Add(Customer customer)
        {
            odataOrderContext.Customers.Add(customer);
            await odataOrderContext.SaveChangesAsync();
            return Ok(customer);
        }
        private readonly List<string> demoCustomers = new List<string>
        {
            "Foo",
            "Bar",
            "Acme",
            "King of Tech",
            "Awesomeness"
        };

        private readonly List<string> demoProducts = new List<string>
        {
            "Bike",
            "Car",
            "Apple",
            "Spaceship"
        };

        private readonly List<string> demoCountries = new List<string>
        {
            "AT",
            "DE",
            "CH"
        };

        [HttpPost]
        [Route("fill")]
        public async Task<IActionResult> Fill()
        {
            var rand = new Random();
            for (var i = 0; i < 10; i++)
            {
                var c = new Customer
                {
                    CustomerName = demoCustomers[rand.Next(demoCustomers.Count)],
                    CountryId = demoCountries[rand.Next(demoCountries.Count)]
                };
                odataOrderContext.Customers.Add(c);

                for (var j = 0; j < 10; j++)
                {
                    var o = new Order
                    {
                        OrderDate = DateTime.UtcNow,
                        Product = demoProducts[rand.Next(demoProducts.Count)],
                        Quantity = rand.Next(1, 5),
                        Revenue = rand.Next(100, 5000),
                        Customer = c
                    };
                    odataOrderContext.Orders.Add(o);
                }
            }

            await odataOrderContext.SaveChangesAsync();
            return Ok();
        }
    }
}
