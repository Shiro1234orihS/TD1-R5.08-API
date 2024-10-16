using TD1_code.Models.DTO;

namespace TD1_code.Respository
{
    public interface IDataDtoTypeProduit
    {
        #region DPO
        Task<IEnumerable<TypeProduitDto>> GetAllAsyncTypeProduitDto();
        Task<TypeProduitDto> GetByIdAsyncTypeProduitDetailDto(int id);
        #endregion
    }
}
