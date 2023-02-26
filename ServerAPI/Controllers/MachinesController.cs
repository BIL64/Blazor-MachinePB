using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ServerAPI.Data;
using ServerAPI.Entities;

namespace ServerAPI.Controllers
{
    [Route("api/Machine")]
    [ApiController]
    public class MachinesController : ControllerBase
    {
        public readonly ServerAPIContext _context;

        public MachinesController(ServerAPIContext context)
        {
            _context = context;
        }

        // GET: api/Machines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Machine>>> GetMachine()
        {
          if (_context.Machine == null)
          {
              return NotFound();
          }
            return await _context.Machine.ToListAsync();
        }

        // GET: api/Machines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Machine>> GetMachine(string id)
        {
          if (_context.Machine == null)
          {
              return NotFound();
          }
            var machine = await _context.Machine.FindAsync(id);

            if (machine == null)
            {
                return NotFound();
            }

            return machine;
        }

        // PUT: api/Machines/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMachine(string id, Machine machine)
        {
            if (id != machine.Id)
            {
                return BadRequest();
            }

            _context.Entry(machine).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MachineExists(id))
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

        // POST: api/Machines
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Machine>> PostMachine(Machine machine)
        {
            if (machine == null)
            {
                return Problem("Entity set 'ServerAPIContext.Machine'  is null.");
            }

            _context.Machine.Add((Machine)machine);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MachineExists(machine.Id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetMachine", new { id = machine.Id }, machine);
        }

        // DELETE: api/Machines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMachine(string id)
        {
            if (_context.Machine == null)
            {
                return NotFound();
            }
            var machine = await _context.Machine.FindAsync(id);
            if (machine == null)
            {
                return NotFound();
            }

            _context.Machine.Remove(machine);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MachineExists(string id)
        {
            return (_context.Machine?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
