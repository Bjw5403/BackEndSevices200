using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public interface ICacheTheCatalog
    {
        Task<catalogModel> GetCatalogAsync();
    }
}