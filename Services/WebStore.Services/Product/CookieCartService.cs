﻿using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using WebStore.Domain.Entities;
using WebStore.Domain.Models;
using WebStore.Domain.ViewModels;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Product
{
    public class CookieCartService : ICartService
    {
        private readonly IProductData _ProductData;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private readonly string _CartName;

        private Cart Cart
        {
            get
            {
                var context = _HttpContextAccessor.HttpContext;
                var cookies = context.Response.Cookies;
                var cookie = context.Request.Cookies[_CartName];
                if (cookie is null)
                {
                    var cart = new Cart();
                    cookies.Append(_CartName, JsonConvert.SerializeObject(cart));
                    return cart;
                }

                ReplaceCookie(cookies, cookie);
                return JsonConvert.DeserializeObject<Cart>(cookie);
            }
            set => ReplaceCookie(_HttpContextAccessor.HttpContext.Response.Cookies, JsonConvert.SerializeObject(value));
        }

        private void ReplaceCookie(IResponseCookies cookies, string cookie)
        {
            cookies.Delete(_CartName);
            cookies.Append(_CartName, cookie, new CookieOptions { Expires = DateTime.Now.AddDays(1) });
        }


        public CookieCartService(IProductData ProductData, IHttpContextAccessor HttpContextAccessor)
        {
            _ProductData = ProductData;
            _HttpContextAccessor = HttpContextAccessor;

            var user = HttpContextAccessor.HttpContext.User;
            var user_name = user.Identity.IsAuthenticated ? user.Identity.Name : null;
            _CartName = $"cart{user_name}";
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
