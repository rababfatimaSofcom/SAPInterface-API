using SApInterface.API.Model.Domain;

namespace SApInterface.API.Repositry
{
    public interface ISectionRepositry
    {
        Task<List<Section>> GetAsync();

        Task<Section> GetSectionAsync(string code);

   
    }
}
