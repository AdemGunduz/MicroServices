using Course.Services.Catalog.Dtos;
using Course.Services.Catalog.Models;
using Course.Shread.Dtos;

namespace Course.Services.Catalog.Services
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();

        Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);

        Task<Response<CategoryDto>> GetById(string id);
    }
}
