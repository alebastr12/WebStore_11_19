using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.ViewModels.BreadCrumbs;
using WebStore.Interfaces.Services;

namespace WebStore.Components
{
    public class BreadCrumbsViewComponent : ViewComponent
    {
        private readonly IProductData _ProductData;

        public BreadCrumbsViewComponent(IProductData ProductData) => _ProductData = ProductData;

        public IViewComponentResult Invoke(BreadCrumbType Type, int id, BreadCrumbType FromType)
        {
            switch (Type)
            {
                default: return View(Array.Empty<BreadCrumbViewModel>());

                case BreadCrumbType.Section:
                    return View(
                        new []
                        {
                            new BreadCrumbViewModel
                            {
                                BreadCrumbType = Type,
                                Id = id.ToString(),
                                Name = _ProductData.GetSectionById(id).Name
                            } 
                        });

                case BreadCrumbType.Brand:
                    return View(
                        new[]
                        {
                            new BreadCrumbViewModel
                            {
                                BreadCrumbType = Type,
                                Id = id.ToString(),
                                Name = _ProductData.GetBrandById(id).Name
                            }
                        });

                case BreadCrumbType.Product:
                    return View(GetProductBreadCrumbs(_ProductData.GetProductById(id), FromType));
            }
        }

        private static IEnumerable<BreadCrumbViewModel> GetProductBreadCrumbs(ProductDTO Product, BreadCrumbType FromType) =>
            new[]
            {
                new BreadCrumbViewModel
                {
                    BreadCrumbType = FromType,
                    Id = FromType == BreadCrumbType.Section
                         ? Product.Section.Id.ToString()
                         : Product.Brand.Id.ToString(),
                    Name = FromType == BreadCrumbType.Section
                           ? Product.Section.Name
                           : Product.Brand.Name
                }, 
                new BreadCrumbViewModel
                {
                    BreadCrumbType = BreadCrumbType.Product,
                    Id = Product.Id.ToString(),
                    Name = Product.Name
                } 
            };
    }
}
