//using Microsoft.AspNetCore.Mvc;

//namespace WebApplication2.Controllers
//{

//    [ApiController]
//    [Route("[controller]")]
//    public class Controller : ControllerBase
//    {

//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication2.Models;
using WebApplication2.DataAccess;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public ClientsController(WarehouseContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Client>> GetClient(long id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // POST: api/Clients
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, client);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutClient(long id, Client client)
        {
            if (id != client.Id)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(id))
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

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClient(long id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(long id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public OrdersController(WarehouseContext context) => _context = context;

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Boxes)
                .ToListAsync();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(long id)
        {
            var order = await _context.Orders
                .Include(o => o.Client)
                .Include(o => o.Boxes)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return NotFound();

            return order;
        }

        // POST: api/Orders
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetOrder), new { id = order.Id }, order);
        }

        // PUT: api/Orders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutOrder(long id, Order order)
        {
            if (id != order.Id)
                return BadRequest();

            _context.Entry(order).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(long id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
                return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OrderExists(long id) => _context.Orders.Any(e => e.Id == id);
    }

    [Route("api/[controller]")]
    [ApiController]
    public class FlowersController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public FlowersController(WarehouseContext context) => _context = context;

        // GET: api/Flowers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flower>>> GetFlowers()
        {
            return await _context.Flowers
                .Include(f => f.Dye)
                .Include(f => f.Ingredients)
                .ToListAsync();
        }

        // POST: api/Flowers
        [HttpPost]
        public async Task<ActionResult<Flower>> PostFlower(Flower flower)
        {
            _context.Flowers.Add(flower);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFlowers), new { id = flower.Id }, flower);
        }

        // Остальные методы (GET by ID, PUT, DELETE) аналогично ClientController
    }

    [Route("api/[controller]")]
    [ApiController]
    public class BoxesController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public BoxesController(WarehouseContext context) => _context = context;

        // GET: api/Boxes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Box>>> GetBoxes()
        {
            return await _context.Boxes
                .Include(b => b.Order)
                .Include(b => b.Flowers)
                .ThenInclude(bf => bf.Flower)
                .ToListAsync();
        }

        // POST: api/Boxes
        [HttpPost]
        public async Task<ActionResult<Box>> PostBox(Box box)
        {
            _context.Boxes.Add(box);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBoxes), new { id = box.Id }, box);
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class DyesController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public DyesController(WarehouseContext context) => _context = context;

        // GET: api/Dyes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Dye>>> GetDyes()
        {
            return await _context.Dyes.ToListAsync();
        }

        // POST: api/Dyes
        [HttpPost]
        public async Task<ActionResult<Dye>> PostDye(Dye dye)
        {
            _context.Dyes.Add(dye);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDyes), new { id = dye.Id }, dye);
        }
    }

    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public IngredientsController(WarehouseContext context) => _context = context;

        // GET: api/Ingredients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients()
        {
            return await _context.Ingredients.ToListAsync();
        }

        // POST: api/Ingredients
        [HttpPost]
        public async Task<ActionResult<Ingredient>> PostIngredient(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetIngredients), new { id = ingredient.Id }, ingredient);
        }
    }



}

