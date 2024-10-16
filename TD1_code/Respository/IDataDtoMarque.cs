using TD1_code.Models.DTO;

namespace TD1_code.Respository
{
    public interface IDataDtoMarque
    {
        Task<IEnumerable<MarqueDto>> GetAllAsyncMarqueDto();
        Task<MarqueDto> GetByIdAsyncMarqueDetailDto(int id);
    }
}
