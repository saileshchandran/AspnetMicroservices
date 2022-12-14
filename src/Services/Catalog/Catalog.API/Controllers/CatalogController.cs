using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CatalogController : ControllerBase
    {
      
        private readonly IProductRepository _repository;
        private readonly ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _repository.GetProducts();
            return Ok(products);

        }

        [HttpGet("{id:length(24)}", Name ="GetProduct")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)System.Net.HttpStatusCode.OK)]
        [ProducesResponseType((int)System.Net.HttpStatusCode.NotFound)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductById(string id)
        {
            var product = await _repository.GetProduct(id);

            if (product == null)
            {
                _logger.LogError($"Product with id:{id} not found !!!");
                return NotFound();
            }

            return Ok(product);
        }

        [Route("[action]/{category}", Name ="GetProductByCategory")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProductByCategory(string category)
        {
            var product = await _repository.GetProductByCategory(category);

            if (product == null)
            {
                _logger.LogError($"Product category with name:{category} not found!!!");
                return NotFound(product);
            }
            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)System.Net.HttpStatusCode.OK)]

        public async Task<ActionResult<IEnumerable<Product>>> CreateProduct([FromBody] Product product)
        {
            await _repository.CreateProduct(product);
            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateProduct([FromBody] Product product)
        {
            return Ok(await _repository.UpdateProduct(product));
        }

        [HttpDelete("{id:length(24)}", Name="DeleteProduct")]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)System.Net.HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteProduct(string Id)
        {
            return Ok(await _repository.DeleteProduct(Id));
        }

    }
}
