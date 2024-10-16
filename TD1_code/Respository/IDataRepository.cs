using Microsoft.AspNetCore.Mvc;
using TD1_code.Models.DTO;

namespace TD1_code.Respository
{
    public interface IDataRepository<TEntity>
    {
        #region GetAllAsync
        // Obtient de manière asynchrone toutes les entités et retourne une liste de ces entités.
        Task<ActionResult<IEnumerable<TEntity>>> GetAllAsync();
        #endregion

        #region GetByIdAsync
        // Récupère une entité spécifique par son ID de manière asynchrone.
        Task<ActionResult<TEntity>> GetByIdAsync(int id);
        #endregion

        #region GetByStringAsync
        // Obtient une entité en utilisant une chaîne comme critère de recherche.
        Task<ActionResult<TEntity>> GetByStringAsync(string str);
        #endregion

        #region AddAsync
        // Ajoute de manière asynchrone une nouvelle entité au référentiel.
        Task AddAsync(TEntity entity);
        #endregion

        #region UpdateAsync
        // Met à jour de manière asynchrone une entité existante en la remplaçant par une nouvelle version.
        Task UpdateAsync(TEntity entityToUpdate, TEntity entity);
        #endregion

        #region DeleteAsync
        // Supprime de manière asynchrone une entité spécifiée du référentiel.
        Task DeleteAsync(TEntity entity);
        #endregion

       
    }

}