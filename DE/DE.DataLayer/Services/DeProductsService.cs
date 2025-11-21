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

        public async Task AddAsync(DeProductDto entity)
            => (await _client.PostAsJsonAsync(string.Empty, entity)).EnsureSuccessStatusCode();

        public async Task DeleteAsync(string id)
            => (await _client.DeleteAsync(id)).EnsureSuccessStatusCode();

        public async Task<List<DeProductDto?>?> GetAllAsync()
            => await _client.GetFromJsonAsync<List<DeProductDto?>?>("");

        public async Task<DeProductDto?> GetAsync(string id)
            => await _client.GetFromJsonAsync<DeProductDto>(id);

        public async Task UpdateAsync(DeProductDto entity)
            => (await _client.PutAsJsonAsync(entity.Id, entity)).EnsureSuccessStatusCode();
    }
}
