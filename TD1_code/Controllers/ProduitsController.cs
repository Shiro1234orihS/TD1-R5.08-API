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
    public class ProduitsController : Controller
    {
        private readonly IDataRepository<Produit> _produit;

        public ProduitsController(IDataRepository<Produit> produit)
        {
            this._produit = produit;
        }

        [HttpGet]
        [ActionName("GetProduits")]
        // GET: produits
        public async Task<ActionResult<IEnumerable<Produit>>> Getproduits()
        {
            return await _produit.GetAllAsync();
        }

        // GET: produits/Details/5
        [HttpGet("{id}")]
        [ActionName("GetProduitById")]
        public async Task<ActionResult<Produit>> GetproduitById(int id)
        {
            var produit = await _produit.GetByIdAsync(id);

            if (produit == null)
            {
                return NotFound();
            }
            return produit;
        }

        // PUT: api/PutProduit/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduit(int id, Produit produit)
        {
            if (id != produit.IdProduit)
            {
                return BadRequest();
            }
            var produitToUpdate = await _produit.GetByIdAsync(id);
            if (produitToUpdate == null)
            {
                return NotFound();
            }
            else
            {
                await _produit.UpdateAsync(produitToUpdate.Value, produit);
                return NoContent();
            }
        }

        // POST: api/PostProduit
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ActionName("PostProduit")]
        public async Task<ActionResult<Produit>> PostProduit(Produit produit)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _produit.AddAsync(produit);
            return CreatedAtAction("GetProduitById", new { id = produit.IdProduit }, produit); // GetById : nom de l’action
        }

        // DELETE: api/DeleteProduit/5
        [HttpDelete("{id}")]
        [ActionName("DeleteProduit")]
        public async Task<IActionResult> DeleteProduit(int id)
        {
            var produit = await _produit.GetByIdAsync(id);
            if (produit == null)
            {
                return NotFound();
            }
            await _produit.DeleteAsync(produit.Value);
            return NoContent();
        }
    }
}
