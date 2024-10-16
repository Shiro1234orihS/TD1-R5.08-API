using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TD1_code.Models.DTO;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;

namespace TD1_code.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MarquesController : Controller
    {
        private readonly IDataRepository<Marque> _marque;
        private readonly IDataDtoMarque _dataDPO;

        public MarquesController(IDataRepository<Marque> Marque, IDataDtoMarque dataDPO)
        {
            this._marque = Marque;
            this._dataDPO = dataDPO;
        }

        [HttpGet]
        [ActionName("GetMarques")]
        // GET: Marques
        public async Task<ActionResult<IEnumerable<Marque>>> GetMarques()
        {
            return await _marque.GetAllAsync();
        }

        // GET: Marques/Details/5
        [HttpGet("{id}")]
        [ActionName("GetMarqueById")]
        public async Task<ActionResult<Marque>> GetMarqueById(int id)
        {
            var marque = await _marque.GetByIdAsync(id);

            if(marque ==  null)
            {
                //return NotFound("Erreur : La marque avec cet ID n'a pas été trouvé. Veuillez vérifier l'URL ou les attributs fournis. Détails : " + ModelState);
                return NotFound();
            }
            return marque;
        }

        // GET: Marques/GeDpoMarque

        [HttpGet]
        [ActionName("GeDpoMarque")]
        public async Task<ActionResult<IEnumerable<MarqueDto>>> GeDpoMarque()
        {
            var Marques = await _dataDPO.GetAllAsyncMarqueDto();

            if (Marques == null || !Marques.Any())
            {
                //return NotFound("Erreur : Aucun Marque trouvé.");
                return NotFound();
            }

            return Ok(Marques);
        }


        // GET: Marques/Details/5
        [HttpGet("{id}")]
        [ActionName("MarqueDetailID")]
        public async Task<MarqueDto> GeMarqueDetailById(int id)
        {
            var Marque = await _dataDPO.GetByIdAsyncMarqueDetailDto(id);
            //if (Marque == null)
            //{
            //    return NotFound("Erreur : Le Marque avec cet ID n'a pas été trouvé. Veuillez vérifier l'URL ou les attributs fournis.");
            //}
            return Marque;
        }

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMarque(int id, Marque marque)
        {
            if (id != marque.IdMarque)
            {
                return BadRequest();
            }
            var marqueToUpdate = await _marque.GetByIdAsync(id);
            if (marqueToUpdate == null)
            {
                //return NotFound("Erreur : La marque avec cet ID n'a pas été trouvé. Veuillez vérifier l'URL ou les attributs fournis. Détails : " + ModelState);
                return NotFound();
            }
            else
            {
                await _marque.UpdateAsync(marqueToUpdate.Value, marque);
                return BadRequest("Erreur : L'ID fourni dans l'URL ne correspond pas à l'ID du Marque. Veuillez vérifier les données saisies. Détails : " + ModelState);
            }
        }

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ActionName("PostMarque")]
        public async Task<ActionResult<Marque>> PostMarque(Marque marque)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Erreur : L'ID fourni dans l'URL ne correspond pas à l'ID du Marque. Veuillez vérifier les données saisies. Détails : " + ModelState);
            }
            await _marque.AddAsync(marque);
            return CreatedAtAction(nameof(GetMarqueById), new { id = marque.IdMarque }, marque); // GetById : nom de l’action
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        [ActionName("DeleteMarque")]
        public async Task<IActionResult> DeleteMarque(int id)
        {
            var marque = await _marque.GetByIdAsync(id);
            if (marque == null)
            {
                return NotFound("Erreur : La marque avec cet ID n'a pas été trouvé. Veuillez vérifier l'URL ou les attributs fournis. Détails : " + ModelState);
            }
            await _marque.DeleteAsync(marque.Value);
            return NoContent();
        }
    }
}
