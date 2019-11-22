using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Formatters.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using WebStore.Domain.Models;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;
using WebStore.Services.Product;
using Assert = Xunit.Assert;

namespace WebStore.Services.Tests.Product
{
    [TestClass]
    public class CartServiceTest
    {
        [TestMethod]
        public void Cart_Class_ItemsCount_Returns_Correct_Quantity()
        {
            const int expected_count = 4;

            var cart = new Cart
            {
                Items = new List<CartItem>
                {
                    new CartItem { ProductId = 1, Quantity = 1 },
                    new CartItem { ProductId = 2, Quantity = 3 },
                }
            };

            var actual_cout = cart.ItemsCount;

            Assert.Equal(expected_count, actual_cout);
        }

        [TestMethod]
        public void CartViewModel_Returns_Correct_ItemsCount()
        {
            const int expected_count = 4;

            var cart_view_model = new CartViewModel
            {
                Items = new Dictionary<ProductViewModel, int>
                {
                    { new ProductViewModel {Id = 1, Name = "Product 1", Price = 0.5m}, 1 },
                    { new ProductViewModel {Id = 2, Name = "Product 2", Price = 1.5m}, 3 },
                }
            };

            var actual_count = cart_view_model.ItemsCount;

            Assert.Equal(expected_count, actual_count);
        }

        [TestMethod]
        public void CartService_AddToCart_WorkCorrect()
        {
            var cart = new Cart
            {
                Items = new List<CartItem>()
            };

            var product_data_mock = new Mock<IProductData>();

            var cart_service = new CookieCartService(product_data_mock.Object);
        }
    }
}
