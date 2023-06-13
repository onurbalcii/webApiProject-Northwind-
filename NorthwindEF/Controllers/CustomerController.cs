using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using System.Diagnostics.Metrics;

namespace NorthwindEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly NorthwindDbContext _context;

        public CustomerController(NorthwindDbContext context)
        {
            _context = context;
        }

        /*[HttpGet]
        public ActionResult<List<Customer>> Index()
        {
            var customers = _context.Customers
                // .Where(x => x.CustomerId == 1)
                .ToList();
            return Ok(customers);
        }*/

        [HttpGet]
        public List<string> GetCustomers()
        {
            List<string> customers = new List<string>();
            NpgsqlConnection cn = new NpgsqlConnection();
            cn.ConnectionString = "Server=127.0.0.1;Port=5432;Database=Northwind;User Id=postgres;Password=12345";
            cn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT company_name, contact_name, country  FROM customers", cn);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            string CompanyName = "";
            string ContactName = "";
            string Country = "";
            while (dr.Read())
            {
                CompanyName = dr.GetString(0);
                customers.Add(CompanyName);
                ContactName = dr.GetString(1);
                customers.Add(ContactName);
                Country = dr.GetString(2);
                customers.Add(Country);
            }
            dr.Close();
            cn.Close();
            return customers;
        }

        [HttpPost]
        public ActionResult<Customer> Edit(string customerId, Customer updatedCustomer)
        {
            var existingCustomer = _context.Customers.FirstOrDefault(x => x.CustomerId == customerId);
            if (existingCustomer == null)
            {
                return NotFound();
            }

            existingCustomer.CompanyName = updatedCustomer.CompanyName;
            existingCustomer.ContactName = updatedCustomer.ContactName;
            existingCustomer.Country = updatedCustomer.Country;

            _context.SaveChanges();

            return Ok(existingCustomer);
        }
    }
}
