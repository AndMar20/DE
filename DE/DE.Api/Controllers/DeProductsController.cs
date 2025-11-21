using DE.DataLayer.Context;
using DE.DataLayer.DTOs.DeProducts;
using DE.DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DE.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeProductsController(AppDbContext context) : ControllerBase
    {
        // GET: api/DeProducts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DeProductDto>>> GetDeProducts()
        {
            var products = await context.DeProducts
                .Include(p => p.Manufacturer)
                .Include(p => p.Supplier)
                .ToListAsync();
            if (!products.Any())
                return NotFound();

            // Формируем базовый URL: https://localhost:7277
            var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";

            var productsDto = products.Select(p => p.ToDto(baseUrl)).ToList();
            return Ok(productsDto);
        }

        // GET: api/DeProducts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DeProduct>> GetDeProduct(string id)
        {
            var deProduct = await context.DeProducts.FindAsync(id);

            if (deProduct == null)
            {
                return NotFound();
            }

            return deProduct;
        }

        // PUT: api/DeProducts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDeProduct(string id, DeProduct deProduct)
        {
            if (id != deProduct.Id)
            {
                return BadRequest();
            }

            context.Entry(deProduct).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeProductExists(id))
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

        // POST: api/DeProducts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DeProduct>> PostDeProduct(DeProduct deProduct)
        {
            context.DeProducts.Add(deProduct);
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DeProductExists(deProduct.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDeProduct", new { id = deProduct.Id }, deProduct);
        }

        // DELETE: api/DeProducts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDeProduct(string id)
        {
            var deProduct = await context.DeProducts.FindAsync(id);
            if (deProduct == null)
            {
                return NotFound();
            }

            context.DeProducts.Remove(deProduct);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private bool DeProductExists(string id)
        {
            return context.DeProducts.Any(e => e.Id == id);
        }
    }
}
