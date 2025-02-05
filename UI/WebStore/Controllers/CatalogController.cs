﻿using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Infrastructure.Map;
using WebStore.Interfaces.Services;

namespace WebStore.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductData _ProductData;

        public CatalogController(IProductData ProductData) => _ProductData = ProductData;

        public IActionResult Shop(int? SectionId, int? BrandId)
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                SectionId = SectionId,
                BrandId = BrandId
            });

            var catalog_model = new CatalogViewModel
            {
                BrandId = BrandId,
                SectionId = SectionId,
                Products = products
                   .Select(p => new Product
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Order = p.Order,
                        ImageUrl = p.ImageUrl,
                        BrandId = p.Brand?.Id,
                        Brand = p.Brand is null ? null : new Brand
                        {
                            Id = p.Brand.Id,
                            Name = p.Brand.Name
                        }
                    })
                   .Select(ProductViewModelMapper.CreateViewModel)
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
    }
}