using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Product
{
    public class CartService : ICartService
    {
        private readonly IProductData _ProductData;
        private readonly ICartStore _CartStore;

        private Cart Cart { get => _CartStore.Cart; set => _CartStore.Cart = value; }


        public CartService(IProductData ProductData, ICartStore CartStore)
        {
            _ProductData = ProductData;
            _CartStore = CartStore;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;
            if (item.Quantity > 0) item.Quantity--;
            if (item.Quantity == 0) cart.Items.Remove(item);

            Cart = cart;
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);
            if (item is null) return;
            cart.Items.Remove(item);

            Cart = cart;
        }

        public void RemoveAll()
        {
            var cart = Cart;
            cart.Items.Clear();
            Cart = cart;
        }

        public void AddToCart(int id)
        {
            var cart = Cart;
            var item = cart.Items.FirstOrDefault(i => i.ProductId == id);

            if (item is null)
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });
            else
                item.Quantity++;

            Cart = cart;
        }

        public CartViewModel TransformCart()
        {
            var products = _ProductData.GetProducts(new ProductFilter
            {
                Ids = Cart.Items.Select(item => item.ProductId).ToList()
            });

            var products_view_models = products.Products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                ImageUrl = p.ImageUrl,
                Brand = p.Brand?.Name
            });

            return new CartViewModel
            {
                Items = Cart.Items.ToDictionary(
                    x => products_view_models.First(p => p.Id == x.ProductId),
                    x => x.Quantity)
            };

        }
    }
}
