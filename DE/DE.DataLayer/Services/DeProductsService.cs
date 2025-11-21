using DE.DataLayer.DTOs.DeProducts;
using System.Net.Http.Json;

namespace DE.DataLayer.Services
{
    public class DeProductsService : IService<DeProductDto>
    {
        private readonly HttpClient _client;
        private readonly string _url = "https://localhost:7277/api/DeProducts";

        public DeProductsService(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri(_url);
        }

        public Task AddAsync(DeProductDto entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DeProductDto?>?> GetAllAsync()
            => await _client.GetFromJsonAsync<List<DeProductDto?>?>("");

        public Task<DeProductDto?> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(DeProductDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
