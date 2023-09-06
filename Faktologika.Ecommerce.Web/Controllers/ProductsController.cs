using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
// using System.Web.Http;

namespace Faktologika.Ecommerce.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    // public class ProductsController : ApiController
    {
        private readonly CatalogDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(CatalogDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProduct()
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            var result = await _context.Product.Select(x => new ProductDto(x)).ToListAsync();
            return result;
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return new ProductDto(product);
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, ProductEditModel model)
        {
            // if (id != productDto.Id)
            // {
            //     return BadRequest();
            // }
            var product = new Product();
            _mapper.Map(model, product);
            product.Id = id;

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductDto>> PostProduct(ProductCreateModel model)
        {
            // var strBody = req.Content.ReadAsStringAsync().Result;
            // var dto = JsonConvert.DeserializeObject<ProductDto>(strBody);
            if (_context.Product == null)
            {
                return Problem("Entity set 'CatalogDbContext.Product'  is null.");
            }

            var product = new Product();
            _mapper.Map(model, product);

            if (product.Price > 1000 || product.Price <= 0)
            {
                return Problem("Price must be between 0 and 1000");
            }
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduct", new { id = product.Id }, new ProductDto(product));
        }

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (_context.Product == null)
            {
                return NotFound();
            }
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
