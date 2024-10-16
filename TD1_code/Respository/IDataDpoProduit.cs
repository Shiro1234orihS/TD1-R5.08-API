﻿using TD1_code.Models.DTO;

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
