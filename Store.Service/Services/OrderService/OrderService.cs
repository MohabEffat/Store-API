﻿using AutoMapper;
using Store.Data.Entities;
using Store.Data.Entities.OrderEntities;
using Store.Repository.Interfaces;
using Store.Repository.Specs.OrderSpecs;
using Store.Service.Services.BasketService;
using Store.Service.Services.OrderService.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Service.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IBasketService _basketService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(
            IBasketService basketService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _basketService = basketService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<OrderDetailsDto> CreateOrderAsync(OrderDto input)
        {
            var basket = await _basketService.GetBasketAsync(input.BasketId);

            if (basket is null)
                throw new Exception("Basket NOT Exist");

            #region Fill Order Item List With Items In The Basket
            var orderItems = new List<OrderItemDto>();

            foreach (var item in basket.basketItems)
            {
                var productItem = _unitOfWork.Repository<Product, int>().GetByIdAsync(item.ProductID);

                if (productItem is null)
                    throw new Exception($"Product With Id : {item.ProductID} Does NOT Exist");

                var itemOrdered = new ProductItem
                {
                    PictureUrl = item.PictureUrl,
                    ProductName = item.ProductName,
                    ProductId = item.ProductID
                };

                var orderItem = new OrderItem
                {
                    ProductItem = itemOrdered,
                    Price = item.Price,
                    Quantity = item.Quantity
                };

                var mappedOrderItem = _mapper.Map<OrderItemDto>(orderItem);

                orderItems.Add(mappedOrderItem);
            }
            #endregion

            #region Get Delivery Method
            var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod, int>().GetByIdAsync(input.DeliveryMethodId);

            if (deliveryMethod is null)
                throw new Exception("Delivery Method Is NOT Provided");


            #endregion

            #region Calcualte Subtotal
            var Subtotal = orderItems.Sum(item => item.Quantity * item.Price);
            #endregion

            #region Create Order
            var mappedShippingAddress = _mapper.Map<ShippingAddress>(input.ShippingAddreess);

            var mappedOrderItems = _mapper.Map<List<OrderItem>>(orderItems);

            var order = new Order
            {
                DeliveryMethodId = deliveryMethod.Id,
                ShippingAddress = mappedShippingAddress,
                BuyerEmail = input.BuyerEmail,
                BasketId = input.BasketId,
                Items = mappedOrderItems,
                SubTotal = Subtotal
            };

            await _unitOfWork.Repository<Order, Guid>().AddAsync(order);

            await _unitOfWork.CompleteAsync();

            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);

            return mappedOrder;
            #endregion

        }

        public async Task<IReadOnlyList<OrderDetailsDto>> GetAllOrderForUserAsync(string buyerEmail)
        {
            var specs = new OrderWithItemSpecifications(buyerEmail);

            var orders = await _unitOfWork.Repository<Order, Guid>().GetAllWithSpecificationsAsync(specs);

            if (!orders.Any())
                throw new Exception("You Dont Have Any Orders YET");

            var mappedOrders = _mapper.Map<List<OrderDetailsDto>>(orders);    

            return mappedOrders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
            => await _unitOfWork.Repository<DeliveryMethod, int>().GetAllAsync();


        public async Task<OrderDetailsDto> GetOrderByIdAsync(Guid id)
        {
            var specs = new OrderWithItemSpecifications(id);

            var order = await _unitOfWork.Repository<Order, Guid>().GetWithSpecificationsByIdAsync(specs);

            if (order is null)
                throw new Exception($"There Is No Order With Id :{id}");

            var mappedOrder = _mapper.Map<OrderDetailsDto>(order);

            return mappedOrder;
        }
    }
}
