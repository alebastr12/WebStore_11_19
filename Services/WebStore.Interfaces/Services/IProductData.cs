using System.Collections.Generic;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;

namespace WebStore.Interfaces.Services
{
    /// <summary>Сервис товаров</summary>
    public interface IProductData
    {
        IEnumerable<Section> GetSections();

        Section GetSectionById(int id);

        IEnumerable<Brand> GetBrands();

        Brand GetBrandById(int id);

        PagedProductDTO GetProducts(ProductFilter Filter);

        ProductDTO GetProductById(int id);
    }
}
