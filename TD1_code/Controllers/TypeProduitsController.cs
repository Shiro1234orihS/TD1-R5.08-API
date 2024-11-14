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
    public class TypeProduitsController : Controller
    {
        private readonly IDataRepository<TypeProduit> _typeProduit;
        private readonly IDataDtoTypeProduit _dataDPO;

        public TypeProduitsController(IDataRepository<TypeProduit> TypeProdui, IDataDtoTypeProduit dataDPO)
        {
            this._typeProduit = TypeProdui;
            _dataDPO = dataDPO;
        }

        [HttpGet]
        [ActionName("GettypeProduits")]
        // GET: typeProduits
        public async Task<ActionResult<IEnumerable<TypeProduit>>> GettypeProduits()
        {
            return await _typeProduit.GetAllAsync();
        }

        // GET: typeProduits/Details/5
        [HttpGet("{id}")]
        [ActionName("GettypeProduitById")]
        public async Task<ActionResult<TypeProduit>> GettypeProduitById(int id)
        {
            var typeProduit = await _typeProduit.GetByIdAsync(id);

            if (typeProduit == null)
            {
                return NotFound();
            }
            return typeProduit;
        }
        // GET: TypeProduits/GeDpoTypeProduit
        [HttpGet]
        [ActionName("GeDpoTypeProduit")]
        public async Task<ActionResult<IEnumerable<TypeProduitDto>>> GeDpoTypeProduit()
        {
            var TypeProduits = await _dataDPO.GetAllAsyncTypeProduitDto();

            if (TypeProduits == null || !TypeProduits.Any())
            {
                //return NotFound("Erreur : Aucun TypeProduit trouvé.");
                return NotFound();
            }

            return Ok(TypeProduits);
        }


        // GET: TypeProduits/Details/5
        [HttpGet("{id}")]
        [ActionName("TypeProduitDetailID")]
        public async Task<TypeProduitDto> GeTypeProduitDetailById(int id)
        {
            var TypeProduit = await _dataDPO.GetByIdAsyncTypeProduitDetailDto(id);
            //if (TypeProduit == null)
            //{
            //    return NotFound("Erreur : Le TypeProduit avec cet ID n'a pas été trouvé. Veuillez vérifier l'URL ou les attributs fournis.");
            //}
            return TypeProduit;
        }

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTypeProduit(int id, TypeProduit typeProduit)
        {
            if (id != typeProduit.IdTypeProduit)
            {
                return BadRequest("Erreur : L'ID fourni dans l'URL ne correspond pas à l'ID du type-produit. Veuillez vérifier les données saisies. Détails : " + ModelState);
            }
            var typeProduitToUpdate = await _typeProduit.GetByIdAsync(id);
            if (typeProduitToUpdate == null)
            {
                //return NotFound("Erreur : La type-produit avec cet ID n'a pas été trouvé. Veuillez vérifier l'URL ou les attributs fournis. Détails : " + ModelState);
                return NotFound();
            }
            else
            {
                await _typeProduit.UpdateAsync(typeProduitToUpdate.Value, typeProduit);
                return NoContent();
            }
        }

        // POST: api/Utilisateurs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ActionName("PostTypeProduit")]
        public async Task<ActionResult<TypeProduit>> PostTypeProduit(TypeProduit typeProduit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Erreur : L'ID fourni dans l'URL ne correspond pas à l'ID du type-produit. Veuillez vérifier les données saisies. Détails : " + ModelState);
            }
            await _typeProduit.AddAsync(typeProduit);
            return CreatedAtAction("GettypeProduitById", new { id = typeProduit.IdTypeProduit }, typeProduit); // GetById : nom de l’action
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        [ActionName("DeleteTypeProduit")]
        public async Task<IActionResult> DeleteTypeProduit(int id)
        {
            var typeProduit = await _typeProduit.GetByIdAsync(id);
            if (typeProduit == null)
            {
                return NotFound();
                return NotFound("Erreur : La type-produit avec cet ID n'a pas été trouvé. Veuillez vérifier l'URL ou les attributs fournis. Détails : " + ModelState);
            }
            await _typeProduit.DeleteAsync(typeProduit.Value);
            return NoContent();
        }
    }
}
