using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace NorthwindEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly NorthwindDbContext _context;

        public  EmployeeController(NorthwindDbContext context)
        {
            _context = context;
        }

        /*[HttpGet]
         public ActionResult<List<Employee>> Index()
         {
             var employees = _context.Employees
                 //.Where(x => x.EmployeeId == 1)
                 .ToList();
             return Ok(employees);
          } */

        [HttpGet]
        public List<string> GetEmployees()
        {
            List<string> employees = new List<string>();
            NpgsqlConnection cn = new NpgsqlConnection();
            cn.ConnectionString = "Server=127.0.0.1;Port=5432;Database=Northwind;User Id=postgres;Password=12345";
            cn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT first_name, last_name, title, city, address FROM employees", cn) ;
            NpgsqlDataReader dr = cmd.ExecuteReader();
            string FirstName = "";
            string LastName = "";
            string Title = "";
            string City = "";
            string Address = "";
            while (dr.Read()) 
            {
                FirstName = dr.GetString(0);
                employees.Add(FirstName);
                LastName = dr.GetString(1);
                employees.Add(LastName);
                Title = dr.GetString(2);
                employees.Add(Title);
                City = dr.GetString(3);
                employees.Add(City);
                Address = dr.GetString(4);
                employees.Add(Address);
            }
            dr.Close();
            cn.Close();
            return employees;
        }

        [HttpPost]
        public ActionResult<Employee> Edit(int employeeId, Employee updatedEmployee)
        {
            var existingEmployee = _context.Employees.FirstOrDefault(x => x.EmployeeId == employeeId);
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

            return Ok(existingEmployee);
        }
    }
}
