using SApInterface.API.Model.Domain;
using System.Data;

namespace SApInterface.API.Repositry
{
    public interface IProductRepositry
    {
        //Task<List<Section>> GetAsync();

        //Task<Section> GetSectionAsync(string code);
        string AddAsync(Product product, DataSet Mdsdataset);

    }
}
