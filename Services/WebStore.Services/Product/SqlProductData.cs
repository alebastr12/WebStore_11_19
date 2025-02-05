﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;

namespace WebStore.Services.Product
{
    public class SqlProductData : IProductData
    {
        private readonly WebStoreContext _db;

        public SqlProductData(WebStoreContext DB) => _db = DB;

        public IEnumerable<Section> GetSections() => _db.Sections
            //.Include(s => s.Products)                             // Грабли
            .AsEnumerable();

        public IEnumerable<Brand> GetBrands() => _db.Brands
            //.Include(brand => brand.Products)                     // Грабли
            .AsEnumerable();

        public IEnumerable<ProductDTO> GetProducts(ProductFilter Filter)
        {
            IQueryable<Domain.Entities.Product> products = _db.Products;

            if (Filter?.SectionId != null)
                products = products.Where(product => product.SectionId == Filter.SectionId);

            if (Filter?.BrandId != null)
                products = products.Where(product => product.BrandId == Filter.BrandId);

            return products.AsEnumerable().Select(p => new ProductDTO
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
            var p = _db.Products
               .Include(product => product.Brand)
               .Include(product => product.Section)
               .FirstOrDefault(product => product.Id == id);
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
