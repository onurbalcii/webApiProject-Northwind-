using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Npgsql;

namespace NorthwindEF.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly NorthwindDbContext _context;

        public ProductController(NorthwindDbContext context)
        {
            _context = context;
        }

        /*[HttpGet]
        public ActionResult<List<Product>> Index()
        {
            var products = _context.Products
                //.Where(x => x.EmployeeId == 1)
                .ToList();
            return Ok(products);
        }*/

        [HttpGet]
        public List<string> GetProducts()
        {
            List<string> products = new List<string>();
            NpgsqlConnection cn = new NpgsqlConnection();
            cn.ConnectionString = "Server=127.0.0.1;Port=5432;Database=Northwind;User Id=postgres;Password=12345";
            cn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand("SELECT product_name, units_in_stock, unit_price FROM products", cn);
            NpgsqlDataReader dr = cmd.ExecuteReader();
            string ProductName = "";
            int UnitStock = 0;
            float UnitPrice = 0;

            while (dr.Read())
            {
                ProductName = dr.GetString(0);
                products.Add(ProductName);
                UnitStock = dr.GetInt32(1);
                products.Add(UnitStock.ToString());
                UnitPrice = dr.GetFloat(2);
                products.Add(UnitPrice.ToString());
            }
            dr.Close();
            cn.Close();
            return products;
        }


        [HttpPost]
        public ActionResult<Product> Edit(int productId, Product updatedProduct)
        {
            var existingProduct = _context.Products.FirstOrDefault(x => x.ProductId == productId);
            if (existingProduct == null)
            {
                return NotFound();
            }

            existingProduct.ProductName = updatedProduct.ProductName;
            existingProduct.UnitStock = updatedProduct.UnitStock;
            existingProduct.UnitPrice = updatedProduct.UnitPrice;

            _context.SaveChanges();

            return Ok(existingProduct);
        }
    }
}
