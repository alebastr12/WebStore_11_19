using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.DTO.Products
{
    /// <summary>Информация по товару</summary>
    public class ProductDTO : INamedEntity, IOrderedEntity
    {
        /// <summary>Идентификатор товара</summary>
        public int Id { get; set; }
        /// <summary>Название</summary>
        public string Name { get; set; }
        /// <summary>Порядковый номер сортировки</summary>
        public int Order { get; set; }
        /// <summary>Цена</summary>
        public decimal Price { get; set; }
        /// <summary>Ссылка на изображение</summary>
        public string ImageUrl { get; set; }

        /// <summary>Бренд товара</summary>
        public BrandDTO Brand { get; set; }
    }
}