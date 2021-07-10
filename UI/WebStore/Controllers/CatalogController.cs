﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Infrastructure.Map;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;
        private readonly IConfiguration _Configuration;

        public CatalogController(IProductData ProductData, IConfiguration Configuration)
        {
            _ProductData = ProductData;
            _Configuration = Configuration;
        }

        private const string __PageSize = "PageSize";
        public IActionResult Shop(int? SectionId, int? BrandId, int Page = 1)
        {
            var page_size = int.Parse(_Configuration[__PageSize]);

            var products = _ProductData.GetProducts(new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Page = Page,
                PageSize = page_size
            });

            var catalog_model = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products.Products
                   .Select(ProductMapper.FromDTO)
                   .Select(ProductViewModelMapper.CreateViewModel),
                PageViewModel = new PageViewModel
                {
                    PageSize = page_size,
                    PageNumber = Page,
                    TotalItems = products.TotalCount
                }
            };

            return View(catalog_model);
        }

        public IActionResult ProductDetails(int id)
        {
            var product = _ProductData.GetProductById(id);

            return product is null
                ? (IActionResult) NotFound()
                : View(new ProductViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Order = product.Order,
                    ImageUrl = product.ImageUrl,
                    Brand = product.Brand?.Name
                });
        }

        public IActionResult GetFiltredItems(int? SectionId, int? BrandId, int Page = 1)
        {
            var products = GetProducts(SectionId, BrandId, Page);
            return PartialView("Partial/_FeaturesItems", products);
        }

        private IEnumerable<ProductViewModel> GetProducts(int? SectionId, int? BrandId, int Page)
        {
            var products_model = _ProductData.GetProducts(new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId,
                Page = Page,
                PageSize = int.Parse(_Configuration[__PageSize])
            });

            return products_model.Products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Order = p.Order,
                ImageUrl = p.ImageUrl,
                Brand = p.Brand?.Name ?? string.Empty
            });
        }
    }
}