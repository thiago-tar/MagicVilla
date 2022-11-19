using MagicVilla.API.Data;
using MagicVilla.API.Model;
using MagicVilla.API.Model.Dto;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla.API.Controllers
{
    [Route("api/Villa")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        public ILogger<VillaController> _logger { get; }

        public VillaController(ILogger<VillaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            _logger.LogInformation("Get all villas");
            return Ok(VillaStore.VillaDTOs);
        }

        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0) return BadRequest();

            var villa = VillaStore.VillaDTOs.FirstOrDefault(x => x.Id == id);

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

            if (VillaStore.VillaDTOs.FirstOrDefault(X => X.Name.ToLower().Equals(villa.Name.ToLower())) != null)
            {
                ModelState.AddModelError("Name", "Villa Alredy Exists!");
                return BadRequest(ModelState);
            }

            villa.Id = VillaStore.VillaDTOs.OrderByDescending(x => x.Id).FirstOrDefault()?.Id + 1 ?? 1;
            VillaStore.VillaDTOs.Add(villa);

            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();

            var villa = VillaStore.VillaDTOs.FirstOrDefault(x => x.Id == id);
            if (villa == null) return NotFound();

            VillaStore.VillaDTOs.Remove(villa);

            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateVilla(int id, [FromBody] VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id || id == 0) return BadRequest();

            var villa = VillaStore.VillaDTOs.FirstOrDefault(x => x.Id == villaDTO.Id);

            if (villa == null) return NotFound();

            villa.Name = villaDTO.Name;
            villa.Sfto = villaDTO.Sfto;
            villa.Ocurrency = villaDTO.Ocurrency;

            return NoContent();

        }

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdatePartialVilla(int id, [FromBody] JsonPatchDocument<VillaDTO> patchDTO)
        {
            if (patchDTO == null || id == 0) return BadRequest();

            var villa = VillaStore.VillaDTOs.FirstOrDefault(x => x.Id == id);

            if (villa == null) return NotFound();
            
            patchDTO.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            return NoContent();

        }
    }
}
