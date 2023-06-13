using Microsoft.AspNetCore.Mvc;
using NorthwindMVC.Models;
using System.Diagnostics;

namespace NorthwindMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly NorthwindDbContext _context;

        public HomeController(ILogger<HomeController> logger, NorthwindDbContext context)
        {
            _logger = logger;
            _context = context; 
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async IActionResult EmployeeList()
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:7010/api/Employee";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }

            /*var employees = _context.Employees
                // .Where(x => x.EmployeeId == 1)
                .ToList();
            return View(employees);*/
        }

        [HttpGet]
        public ActionResult EmployeeEdit(int Id)
        {
            var employee = _context.Employees.Where(x => x.EmployeeId == Id).FirstOrDefault();
            return View(employee);
        }

        [HttpPost]
        public ActionResult EmployeeEdit(Employee updatedEmployee)
        {
            if (ModelState.IsValid)
            {
                var existingEmployee = _context.Employees.FirstOrDefault(x => x.EmployeeId == updatedEmployee.EmployeeId);
                if (existingEmployee == null)
                {
                    return NotFound();
                }

                existingEmployee.FirstName = updatedEmployee.FirstName;
                existingEmployee.LastName = updatedEmployee.LastName;
                existingEmployee.Title = updatedEmployee.Title;
                existingEmployee.City = updatedEmployee.City;
                existingEmployee.Address = updatedEmployee.Address;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(updatedEmployee);
        }


        public IActionResult CustomerList()
        {
            HttpClient client = new HttpClient();
            string url = "https://localhost:7010/api/Customer";
            var response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                return null;
            }

            /*var customers = _context.Customers
               // .Where(x => x.CustomerId == 1)
               .ToList();
            return View(customers);
        }*/

        [HttpGet]
        public ActionResult CustomerEdit(string Id)
        {
            var customer = _context.Customers.Where(x => x.CustomerId == Id).FirstOrDefault();
            return View(customer);
        }
        [HttpPost]
        public ActionResult CustomerEdit(Customer updatedCustomer)
        {
            if (ModelState.IsValid)
            {
                var existingCustomer = _context.Customers.FirstOrDefault(x => x.CustomerId == updatedCustomer.CustomerId);
                if (existingCustomer == null)
                {
                    return NotFound();
                }

                existingCustomer.CompanyName = updatedCustomer.CompanyName;
                existingCustomer.ContactName = updatedCustomer.ContactName;
                existingCustomer.Country = updatedCustomer.Country;

                _context.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(updatedCustomer);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}