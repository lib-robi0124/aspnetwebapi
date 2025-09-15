using Avenga.NotesApp.Dtos.FruitsDtos;

namespace Avenga.NotesApp.Services.Interfaces
{
    public interface IFruitService
    {
        Task<FruitDto> GetFruitInfoAsync(string fruitname);
        Task<List<FruitDto>> GetFruitsByOrderAsync(string orderName);
        Task<List<FruitDto>> GetFruitsByFamilyAsync(string familyName);
        Task<List<FruitDto>> GetFruitsByGenusAsync(string genusName);
        Task<List<FruitDto>> GetAllFruitsAsync();
    }
}
