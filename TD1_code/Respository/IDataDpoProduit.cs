using TD1_code.Models.DPO;

namespace TD1_code.Respository
{
    public interface IDataDpoProduit
    {
        #region DPO
        Task<IEnumerable<ProduitDto>> GetAllAsyncProduitDto();
        Task<ProduitDetailDto> GetByIdAsyncProduitDetailDto(int id);
        #endregion
    }
}
