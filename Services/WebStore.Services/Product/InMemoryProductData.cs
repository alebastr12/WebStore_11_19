using System.Collections.Generic;
using System.Linq;
using WebStore.DAL.Data;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Product
{
    public class InMemoryProductData : IProductData
    {
        public IEnumerable<Section> GetSections() => TestData.Sections;

        public IEnumerable<Brand> GetBrands() => TestData.Brands;

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter)
        {
            var products = TestData.Products;

            if (Filter?.BrandId != null)
                products = products.Where(product => product.BrandId == Filter.BrandId);
            if (Filter?.SectionId != null)
                products = products.Where(product => product.SectionId == Filter.SectionId);

            return products.Select(p => new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Brand = p.Brand is null ? null : new BrandDTO
                {
                    Id = p.Brand.Id,
                    Name = p.Brand.Name
                }
            });
        }

        public ProductDTO GetProductById(int id)
        {
            var p = TestData.Products.FirstOrDefault(product => product.Id == id);
            return new ProductDTO
            {
                Id = p.Id,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Brand = p.Brand is null ? null : new BrandDTO
                {
                    Id = p.Brand.Id,
                    Name = p.Brand.Name
                }
            };
        }
    }
}
