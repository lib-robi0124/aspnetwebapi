using Avenga.NotesApp.Dtos.FruitsDtos;
using Avenga.NotesApp.Services.Interfaces;
using System.Text.Json;

namespace Avenga.NotesApp.Services.Implementations
{
    public class FruitService : IFruitService
    {
        private readonly HttpClient _httpClient;
        public FruitService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        //private const string BaseUrl = "https://www.fruityvice.com/api/fruit";
        public async Task<List<FruitDto>> GetAllFruitsAsync(string fruitName)
        {
            var response = await _httpClient.GetAsync("https://www.fruityvice.com/api/fruit/all");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<FruitDto>>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                return null;
            }
        }

        public async Task<FruitDto> GetFruitInfoAsync(string fruitname)
        {
            //var response = await _httpClient.GetAsync($"https://www.fruityvice.com/api/fruit/{fruitname}");
            var response = await _httpClient.GetAsync($"https://www.fruityvice.com/api/fruit/all");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<FruitDto>>(json, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                var fruitByName = result.FirstOrDefault(f => f.Name.ToLower() == fruitname.ToLower());
                return fruitByName;
            }
        }

        public Task<List<FruitDto>> GetFruitsFamilyAsync(string familyName)
        {
            throw new NotImplementedException();
        }

        public Task<List<FruitDto>> GetFruitsGenusAsync(string genusName)
        {
            throw new NotImplementedException();
        }

        public Task<List<FruitDto>> GetFruitsOrderAsync(string orderName)
        {
            throw new NotImplementedException();
        }
    }
}
