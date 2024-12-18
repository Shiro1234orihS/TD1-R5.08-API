﻿using System;
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
    public class ProduitsController : Controller
    {
        private readonly IDataRepository<Produit> _produit;
        private readonly IDataDtoProduit _dataDPO;

        public ProduitsController(IDataRepository<Produit> produit ,IDataDtoProduit dataDPO)
        {
            this._produit = produit;
            this._dataDPO = dataDPO;
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
                //return NotFound("Erreur : Le produit avec cet ID n'a pas été trouvé. Veuillez vérifier l'URL ou les attributs fournis.");
                return NotFound();
            }
            return produit;
        }

        // GET: produits/GeDpoProduit

        [HttpGet]
        [ActionName("GetDpoProduit")]
        public async Task<ActionResult<IEnumerable<ProduitDto>>> GetDpoProduit()
        {
            var produits = await _dataDPO.GetAllAsyncProduitDto();

            if (produits == null || !produits.Any())
            {
                //return NotFound("Erreur : Aucun produit trouvé.");
                return NotFound();
            }

            return Ok(produits);
        }


        // GET: produits/Details/5
        [HttpGet("{id}")]
        [ActionName("ProduitDetailID")]
        public async Task<ActionResult<ProduitDetailDto>> GetProduitDetailById(int id)
        {
            var produit = await _dataDPO.GetByIdAsyncProduitDetailDto(id);
            if (produit == null)
            {
                return NotFound(); // Renvoie un 404 si le produit n'est pas trouvé
            }
            return Ok(produit); // Renvoie un 200 avec le produit trouvé
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
                return BadRequest("Erreur : L'ID fourni dans l'URL ne correspond pas à l'ID du produit. Veuillez vérifier les données saisies. Détails : " + ModelState);
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
                //return NotFound("Erreur : Le produit avec cet ID n'a pas été trouvé. Veuillez vérifier l'URL ou les attributs fournis.");
                return NotFound();
            }
            await _produit.DeleteAsync(produit.Value);
            return NoContent();
        }
    }
}
