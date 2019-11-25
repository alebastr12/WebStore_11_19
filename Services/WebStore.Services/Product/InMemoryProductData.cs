using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Data;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Services.Product
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;

        public Section GetSectionById(int id) => GetSections().FirstOrDefault(s => s.Id == id);

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public Brand GetBrandById(int id) => GetBrands().FirstOrDefault(b => b.Id == id);

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter)
        {
            var products = TestData.Products;

            if (Filter?.BrandId != null)
                products = products.Where(product => product.BrandId == Filter.BrandId);
            if (Filter?.SectionId != null)
                products = products.Where(product => product.SectionId == Filter.SectionId);

            return products.Select(ProductMapper.ToDTO);
        }

        public ProductDTO GetProductById(int id) => TestData.Products
           .FirstOrDefault(product => product.Id == id)
           .ToDTO();
    }
}
