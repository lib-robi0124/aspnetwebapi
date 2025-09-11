using Avenga.NotesApp.Dtos.FruitsDtos;

namespace Avenga.NotesApp.Services.Interfaces
{
    public interface IFruitService
    {
        Task<FruitDto> GetFruitInfoAsync(string fruitname);
        Task<List<FruitDto>> GetFruitsOrderAsync(string orderName);
        Task<List<FruitDto>> GetFruitsFamilyAsync(string familyName);
        Task<List<FruitDto>> GetFruitsGenusAsync(string genusName);
        Task<List<FruitDto>> GetAllFruitsAsync();
    }
}
