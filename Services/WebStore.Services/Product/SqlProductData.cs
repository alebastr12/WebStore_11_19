﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Domain.DTO.Products;
using WebStore.Domain.Entities;
using WebStore.Interfaces.Services;
using WebStore.Services.Map;

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

            return products.AsEnumerable().Select(ProductMapper.ToDTO);
        }

        public ProductDTO GetProductById(int id) =>
            _db.Products
               .Include(product => product.Brand)
               .Include(product => product.Section)
               .FirstOrDefault(product => product.Id == id)
               .ToDTO();
    }
}
