using MagicVilla.API.Data;
using MagicVilla.API.Model;
using MagicVilla.API.Model.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MagicVilla.API.Controllers
{
    [Route("api/Villa")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ILogger<VillaController> _logger { get; }

        public VillaController(ILogger<VillaController> logger,
                                ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation("Get all villas");
            return Ok(_db.Villas);
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0) return BadRequest();

            var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null) return NotFound();

            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<VillaDTO>> CreateVilla([FromBody] VillaDTO villa)
        {
            if (villa == null) return BadRequest(villa);

            if (villa.Id > 0) return StatusCode(StatusCodes.Status500InternalServerError);

            if (await _db.Villas.FirstOrDefaultAsync(X => X.Name.ToLower().Equals(villa.Name.ToLower())) != null)
            {
                ModelState.AddModelError("Name", "Villa Alredy Exists!");
                return BadRequest(ModelState);
            }

            await _db.Villas.AddAsync(villa.ConvertToVilla());
            await _db.SaveChangesAsync();

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();

            var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null) return NotFound();

            _db.Villas.Remove(villa);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id || id == 0) return BadRequest();

            _db.Villas.Update(villaDTO.ConvertToVilla());
            await _db.SaveChangesAsync();
            return NoContent();

        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<Villa> patchDTO)
        {
            if (patchDTO == null || id == 0) return BadRequest();

            var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (villa == null) return NotFound();


            patchDTO.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _db.SaveChangesAsync();

            return NoContent();

        }
    }
}
