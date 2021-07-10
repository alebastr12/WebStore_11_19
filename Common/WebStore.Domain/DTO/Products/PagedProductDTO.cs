﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WebStore.Domain.DTO.Products
{
    /// <summary>одна страница товаров каталога</summary>
    public class PagedProductDTO
    {
        /// <summary>Товары страницы</summary>
        public IEnumerable<ProductDTO> Products { get; set; }

        /// <summary>Полное количество товаров в каталоге</summary>
        public int TotalCount { get; set; }
    }
}
