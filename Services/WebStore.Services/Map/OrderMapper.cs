using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.Entities;

namespace WebStore.Services.Map
{
    public static class OrderMapper
    {
        public static OrderDTO ToDTO(this Order order) => order is null ? null : new OrderDTO
        {
            Id = order.Id,
            Name = order.Name,
            Date = order.Date,
            Address = order.Address,
            Phone = order.Phone,
            OrderItems = order.OrderItems.Select(OrderItemMapper.ToDTO)
        };

        public static Order FromDTO(this OrderDTO order) => order is null ? null : new Order
        {
            Id = order.Id,
            Name = order.Name,
            Date = order.Date,
            Address = order.Address,
            Phone = order.Phone,
            OrderItems = order.OrderItems.Select(OrderItemMapper.FomDTO).ToArray()
        };
    }
}
