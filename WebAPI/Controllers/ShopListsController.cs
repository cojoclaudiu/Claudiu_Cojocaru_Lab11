using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopListsController : ControllerBase
    {
        private readonly WebAPIContext _context;

        public ShopListsController(WebAPIContext context)
        {
            _context = context;
        }

        // GET: api/ShopLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ShopList>>> GetShopLists()
        {
            return await _context.ShopLists.ToListAsync();
        }

        // GET: api/ShopLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShopList>> GetShopList(int id)
        {
            var shopList = await _context.ShopLists.FindAsync(id);

            if (shopList == null)
            {
                return NotFound();
            }

            return shopList;
        }

        // PUT: api/ShopLists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShopList(int id, ShopList shopList)
        {
            if (id != shopList.ID)
            {
                return BadRequest();
            }

            _context.Entry(shopList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShopListExists(id))
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

        // POST: api/ShopLists
        [HttpPost]
        public async Task<ActionResult<ShopList>> PostShopList(ShopList shopList)
        {
            _context.ShopLists.Add(shopList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShopList", new { id = shopList.ID }, shopList);
        }

        // DELETE: api/ShopLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShopList(int id)
        {
            var shopList = await _context.ShopLists.FindAsync(id);
            if (shopList == null)
            {
                return NotFound();
            }

            _context.ShopLists.Remove(shopList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShopListExists(int id)
        {
            return _context.ShopLists.Any(e => e.ID == id);
        }
    }
}