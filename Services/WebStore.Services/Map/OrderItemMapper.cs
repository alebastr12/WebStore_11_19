using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
    public static class OrderItemMapper
    {
        public static OrderItemDTO ToDTO(this OrderItem item) => item is null ? null : new OrderItemDTO
        {
            Id = item.Id,
            Price = item.Price,
            Quantity = item.Quantity
        };

        public static OrderItem FomDTO(this OrderItemDTO item) => item is null ? null : new OrderItem
        {
            Id = item.Id,
            Price = item.Price,
            Quantity = item.Quantity
        };
    }
}