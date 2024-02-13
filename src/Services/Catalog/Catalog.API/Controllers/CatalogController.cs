using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private ILogger<CatalogController> _logger;

        public CatalogController(IProductRepository repository, ILogger<CatalogController> logger)
        {
            this._repository = repository;
            this._logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await this._repository.GetProducts();

            return Ok(products);
        }

        [HttpGet("{id:length(24)}",Name ="GetProduct")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Product),(int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductById(string id)
        {
            var product = await this._repository.GetProduct(id);
            if(product == null)
            {
                this._logger.LogError($"Product with id: {id}, not found");
                return NotFound();
            }
            return Ok(product);
        }


        [Route("[action]/{category}", Name = "GetProductByCategory")]
        [HttpGet]

        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> GetProductByCategory(string category)
        {
            var product = await this._repository.GetProductByCategory(category);
            if (product == null)
            {
                this._logger.LogError($"Product with category: {category}, not found");
                return NotFound();
            }
            return Ok(product);
        }


        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> CreateProduct([FromBody] Product product)
        {
            await this._repository.CreateProduct(product);

            return CreatedAtRoute("GetProduct",new {id=product.Id },product);
        }


        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> UpdateProduct([FromBody] Product product)
        {
            return Ok(await this._repository.UpdateProduct(product));
        }
        [HttpDelete("{id:length(24)}",Name ="DeleteProduct")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Product>> DeleteProduct(string id)
        {
            return Ok(await this._repository.DeleteProduct(id));
        }
    }
}
