using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Data;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Product
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<Domain.Entities.Product> GetProducts(ProductFilter Filter)
        {
            var products = TestData.Products;
            if (Filter is null) return products;
            if (Filter.BrandId != null)
                products = products.Where(product => product.BrandId == Filter.BrandId);
            if (Filter.SectionId != null)
                products = products.Where(product => product.SectionId == Filter.SectionId);
            return products;
        }

        public Domain.Entities.Product GetProductById(int id) => TestData.Products.FirstOrDefault(product => product.Id == id);
    }
}
