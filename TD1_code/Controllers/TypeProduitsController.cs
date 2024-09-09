using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TD1_code.Models.EntityFramework;
using TD1_code.Respository;

namespace TD1_code.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TypeProduitsController : Controller
    {
        private readonly IDataRepository<TypeProduit> _typeProduit;

        public TypeProduitsController(IDataRepository<TypeProduit> TypeProdui)
        {
            this._typeProduit = TypeProdui;
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

        // PUT: api/Utilisateurs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGammeMoto(int id, TypeProduit typeProduit)
        {
            if (id != typeProduit.IdTypeProduit)
            {
                return BadRequest();
            }
            var typeProduitToUpdate = await _typeProduit.GetByIdAsync(id);
            if (typeProduitToUpdate == null)
            {
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
        public async Task<ActionResult<TypeProduit>> PostGammeMoto(TypeProduit typeProduit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _typeProduit.AddAsync(typeProduit);
            return CreatedAtAction("GettypeProduitById", new { id = typeProduit.IdTypeProduit }, typeProduit); // GetById : nom de l’action
        }

        // DELETE: api/Utilisateurs/5
        [HttpDelete("{id}")]
        [ActionName("DeleteTypeProduit")]
        public async Task<IActionResult> DeleteGammeMoto(int id)
        {
            var typeProduit = await _typeProduit.GetByIdAsync(id);
            if (typeProduit == null)
            {
                return NotFound();
            }
            await _typeProduit.DeleteAsync(typeProduit.Value);
            return NoContent();
        }
    }
}
