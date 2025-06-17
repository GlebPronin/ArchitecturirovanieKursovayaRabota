using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Net.Http;
using System.Text.Json;

namespace CacheLayer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IDistributedCache _cache;
        private readonly IHttpClientFactory _httpClientFactory;

        public ProductsController(IDistributedCache cache, IHttpClientFactory httpClientFactory)
        {
            _cache = cache;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var cachedData = await _cache.GetStringAsync("products");

            if (!string.IsNullOrEmpty(cachedData))
            {
                return Ok(JsonSerializer.Deserialize<object>(cachedData));
            }

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("http://productservice:8080/products"); // имя в docker-compose

            if (!response.IsSuccessStatusCode)
                return StatusCode(500, "Не удалось получить продукты");

            var content = await response.Content.ReadAsStringAsync();

            await _cache.SetStringAsync("products", content, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
            });

            return Ok(JsonSerializer.Deserialize<object>(content));
        }
    }
}
