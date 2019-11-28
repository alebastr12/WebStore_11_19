using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.Core.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Controllers;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

using Assert = Xunit.Assert;

namespace WebStore.Tests.Controllers
{
    [TestClass]
    public class CatalogControllerTests
    {
        [TestMethod]
        public void ProductDetails_Returns_With_Correct_Item()
        {
            // A-A-A = Arrange - Act - Assert

            #region Arrange

            const int expecetd_id = 1;
            const decimal expected_price = 10m;
            var expected_name = $"Item id {expecetd_id}";
            var expected_brand_name = $"Brand of item {expecetd_id}";

            var product_data_mock = new Mock<IProductData>();

            product_data_mock
               .Setup(p => p.GetProductById(It.IsAny<int>()))
               .Returns<int>(id => new ProductDTO
               {
                   Id = id,
                   Name = $"Item id {id}",
                   ImageUrl = $"Image_id_{id}.png",
                   Order = 0,
                   Price = expected_price,
                   Brand = new BrandDTO
                   {
                       Id = 1,
                       Name = $"Brand of item {id}"
                   }
               });

            var configuration_mock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            var controller = new CatalogController(product_data_mock.Object, configuration_mock.Object);

            #endregion

            #region Act

            var result = controller.ProductDetails(expecetd_id);

            #endregion

            #region Assert

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<ProductViewModel>(view_result.Model);

            Assert.Equal(expecetd_id, model.Id);
            Assert.Equal(expected_name, model.Name);
            Assert.Equal(expected_price, model.Price);
            Assert.Equal(expected_brand_name, model.Brand);

            #endregion
        }

        [TestMethod]
        public void ProductDetails_Return_NotFound_if_Product_not_Exists()
        {
            var product_data_mock = new Mock<IProductData>();

            product_data_mock
               .Setup(p => p.GetProductById(It.IsAny<int>()))
               .Returns(default(ProductDTO));

            var configuration_mock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            var controller = new CatalogController(product_data_mock.Object, configuration_mock.Object);

            var result = controller.ProductDetails(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [TestMethod]
        public void Shop_Returns_Correct_View()
        {
            var products = new[]
            {
                new ProductDTO
                {
                    Id = 1,
                    Name = "Product 1",
                    Order = 0,
                    Price = 10m,
                    ImageUrl = "Product1.png",
                    Brand = new BrandDTO
                    {
                        Id = 1,
                        Name = "Brand of product 1"
                    },
                    Section = new SectionDTO
                    {
                        Id = 1,
                        Name = "Products section id 1"
                    }
                },
                new ProductDTO
                {
                    Id = 2,
                    Name = "Product 2",
                    Order = 1,
                    Price = 20m,
                    ImageUrl = "Product2.png",
                    Brand = new BrandDTO
                    {
                        Id = 1,
                        Name = "Brand of product 2"
                    },
                    Section = new SectionDTO
                    {
                        Id = 1,
                        Name = "Products section id 1"
                    }
                }
            };

            var product_data_mock = new Mock<IProductData>();
            product_data_mock
               .Setup(p => p.GetProducts(It.IsAny<ProductFilter>()))
               .Returns<ProductFilter>(filter => new PagedProductDTO
                {
                    Products = products,
                    TotalCount = products.Length
                });

            var configuration_mock = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();

            var controller = new CatalogController(product_data_mock.Object, configuration_mock.Object);

            const int expected_section_id = 1;
            const int expected_brand_id = 5;

            var result = controller.Shop(expected_section_id, expected_brand_id);

            var view_result = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<CatalogViewModel>(view_result.ViewData.Model);

            Assert.Equal(2, model.Products.Count());
            Assert.Equal(expected_brand_id, model.BrandId);
            Assert.Equal(expected_section_id, model.SectionId);

            Assert.Equal("Brand of product 1", model.Products.First().Brand);
        }
    }
}
