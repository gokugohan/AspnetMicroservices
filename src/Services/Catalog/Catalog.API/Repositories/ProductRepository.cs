using Catalog.API.Data;
using Catalog.API.Entities;
using Catalog.API.Repositories.Interfaces;
using MongoDB.Driver;
using System.Xml.Linq;

namespace Catalog.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;
        public ProductRepository(ICatalogContext context)
        {
            this._context = context??throw new ArgumentNullException(nameof(context));
        }
       

        public async Task<Product> GetProduct(string id)
        {
            return await this._context.Products.Find(m=>m.Id.Equals(id)).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
        {
            FilterDefinition<Product> filter = Builders<Product>
                                                        .Filter.Eq(p=>p.Category.ToLower(), categoryName.ToLower());


            return await this._context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            FilterDefinition<Product> filter = Builders<Product>
                                                        .Filter.Eq(p => p.Name.ToLower(), name.ToLower());

            return await this._context.Products.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts()
        {
            return await this._context.Products.Find(m => true).ToListAsync();
        }

        public async Task CreateProduct(Product product)
        {
            await this._context.Products.InsertOneAsync(product);
        }

        public async Task<bool> DeleteProduct(string id)
        {

            FilterDefinition<Product> filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            
            DeleteResult deleteResult = await this._context.Products.DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            var updateResult = await this._context.Products.ReplaceOneAsync(
                filter: g => g.Id == product.Id,
                replacement: product);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }
    }
}
