namespace LinkUp.Core.Application.Interfaces
{
    public interface IGenericService<DtoModel> where DtoModel : class
    {
        Task<DtoModel?> AddAsync(DtoModel dto);
        Task<bool> DeleteAsync(int id);
        Task<List<DtoModel>> GetAll();
        Task<DtoModel?> GetById(int id);
        Task<DtoModel?> UpdateAsync(DtoModel dto, int id);
    }
}