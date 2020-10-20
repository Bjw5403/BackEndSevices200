using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi.Services
{
    public class CatalogService : ICacheTheCatalog
    {
        public readonly IDistributedCache _cache;
        public CatalogService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<catalogModel> GetCatalogAsync()
        {
            //Ask the cache for the thing
            var catalog = await _cache.GetAsync("catalog");
            string newCatalog = null;

            // if there is a catalog in the cache - return that
            if (catalog == null)
            {
                newCatalog = $"Thiis Catalog was created at {DateTime.Now.ToLongTimeString()}";
                var encodedCatalog = Encoding.UTF8.GetBytes(newCatalog);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddSeconds(15));
                await _cache.SetAsync("catalog", encodedCatalog, options);
            }
            else
            {
                newCatalog = Encoding.UTF8.GetString(catalog);
            }
            return new catalogModel { data = newCatalog };
        }
    }

    public class catalogModel
    {
        public string data { get; set; }
    }
}
