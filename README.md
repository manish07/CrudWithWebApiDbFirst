Open the terminal:

```dotnet new webapi --name CrudWithWebAPI```

Add below packages:

```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.EntityFrameworkCore.Relational
```
Create a database:

```CREATE DATABASE CRUDWithWebAPI```

Add product table:

```sql
CREATE TABLE Products(
    Id int IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Price FLOAT NOT NULL,
    Qty int NOT NULL
)
```

Add Models folder:
Add Product.cs class

```C#
using System.ComponentModel.DataAnnotations;

namespace CrudWithWebAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public int Qty { get; set; }
    }
}
```

Create a folder DataAccess
Add MyAppDbContext.cs class

```C#
using CrudWithWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudWithWebAPI.DataAccess
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }
    }
}
```

In appsettings.json
Add connection string

```C#
"ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=CRUDWithWebAPI;User=sa;Password=dockerStrongPwd123;Integrated Security=False;TrustServerCertificate=True;"
  }
```

Next register ConnectionStrings in Program.cs

```C#
builder.Services.AddDbContext<MyAppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
```

Add Controllers folder
Add ProductController as a api controller

```C#
using CrudWithWebAPI.DataAccess;
using CrudWithWebAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudWithWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly MyAppDbContext _context;

        public ProductController(MyAppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                var products = _context.Products.ToList();

                if(products.Count == 0)
                {
                    return NotFound("Products not available");
                }

                return Ok(products);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var product = _context.Products.Find(id);

                if(product == null)
                {
                    return NotFound("Product not found");
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult Post(Product model)
        {
            try
            {
                _context.Products.Add(model);
                _context.SaveChanges();
                return Ok("Product created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public IActionResult Put(Product model)
        {
            if(model == null || model.Id == 0)
            {
                if(model == null)
                {
                    return BadRequest("Model data is invalid");
                }
                if(model.Id == 0)
                {
                    return BadRequest($"Product Id {model.Id} is invalid");
                }
            }
            try
            {
                var product = _context.Products.Find(model.Id);

                if(product == null)
                {
                    return BadRequest($"Product with {model.Id} is not found");
                }

                product.Name = model.Name;
                product.Price = model.Price;
                product.Qty = model.Qty;
                _context.SaveChanges();
                return Ok("Product details updated.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var product = _context.Products.Find(id);

                if(product == null)
                {
                    return BadRequest($"Product with id {id} not found");
                }

                _context.Products.Remove(product);
                _context.SaveChanges();
                return Ok("Product deleted.");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
```

Run the application:  

```C#
dotnet run
```

Access the url
```
http://localhost:5218/swagger/index.html
```

To see the added api, add service in Program.cs
```
builder.Services.AddControllers();
```

To perform CRUD operation makesure that below is added into Program.cs

```
app.MapControllers();
```



