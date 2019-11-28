using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.ServiceHosting.Controllers
{
    /// <summary>Контроллер товаров</summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase, IProductData
    {
        private readonly IProductData _ProductData;

        public ProductsController(IProductData ProductData) => _ProductData = ProductData;

        /// <summary>Получить все секции</summary>
        /// <returns>Возвращает список секций базы данных</returns>
        [HttpGet("sections")]
        public IEnumerable<Section> GetSections() => _ProductData.GetSections();

        [HttpGet("sections/{id}")]
        public Section GetSectionById(int id) => _ProductData.GetSectionById(id);

        /// <summary>Получить все бренды</summary>
        /// <returns>Возвращает список брендов базы данных</returns>
        [HttpGet("brands")]
        public IEnumerable<Brand> GetBrands() => _ProductData.GetBrands();

        [HttpGet("brands/{id}")]
        public Brand GetBrandById(int id) => _ProductData.GetBrandById(id);

        /// <summary>Выборка товаров по заданному фильтром критерию поиска</summary>
        /// <param name="Filter">Фильтр - критерий поиска товаров</param>
        /// <returns>Список товаров, удовлетворяющий критерию фильтрации</returns>
        [HttpPost, ActionName("Post")]
        public PagedProductDTO GetProducts(ProductFilter Filter) => _ProductData.GetProducts(Filter);

        /// <summary>Получить информацию по товару, заданному своим идентификатором</summary>
        /// <param name="id">Идентификатор товара</param>
        /// <returns>Информация по товару</returns>
        [HttpGet("{id}"), ActionName("Get")]
        public ProductDTO GetProductById(int id) => _ProductData.GetProductById(id);
    }
}