using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Product
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreContext _db;

        public SqlProductData(WebStoreContext DB) => _db = DB;

        public IEnumerable<Section> GetSections() => _db.Sections
            .Include(s => s.Products)
            .AsEnumerable();

        public IEnumerable<Brand> GetBrands() => _db.Brands
            .Include(brand => brand.Products)
            .AsEnumerable();

        public IEnumerable<Domain.Entities.Product> GetProducts(ProductFilter Filter)
        {
            IQueryable<Domain.Entities.Product> products = _db.Products;
            if (Filter is null)
                return products.AsEnumerable();

            if (Filter.SectionId != null)
                products = products.Where(product => product.SectionId == Filter.SectionId);

            if (Filter.BrandId != null)
                products = products.Where(product => product.BrandId == Filter.BrandId);

            return products.AsEnumerable();
        }

        public Domain.Entities.Product GetProductById(int id) =>
            _db.Products
                .Include(product => product.Brand)
                .Include(product => product.Section)
                .FirstOrDefault(product => product.Id == id);
    }
}
