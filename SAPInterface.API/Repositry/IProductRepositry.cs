using SApInterface.API.Model.Domain;

namespace SApInterface.API.Repositry
{
    public interface IProductRepositry
    {
        Task<List<Section>> GetAsync();

        Task<Section> GetSectionAsync(string code);
        string AddAsync(Product section);

    }
}
