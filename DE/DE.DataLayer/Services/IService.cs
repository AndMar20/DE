namespace DE.DataLayer.Services
{
    public interface IService<TDto> where TDto : class
    {
        Task<List<TDto?>?> GetAllAsync();
        Task<TDto?> GetAsync(int id);
        Task UpdateAsync(TDto entity);
        Task AddAsync(TDto entity);
        Task DeleteAsync(int id);
    }
}
