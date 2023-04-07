using System;
using System.Collections.Generic;
using TechZone.Data.Infracstructure;
using TechZone.Data.Repositories;
using TechZone.Model.Models;

namespace TechZone.Service
{
    public interface IOrderService
    {
        bool Create(Order order, List<OrderDetail> orderDetails);
    }

    public class OrderService : IOrderService
    {
        private IOrderRepository _orderRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository, IUnitOfWork unitOfWork)
        {
            this._orderRepository = orderRepository;
            this._orderDetailRepository = orderDetailRepository;
            this._unitOfWork = unitOfWork;
        }

        public bool Create(Order order, List<OrderDetail> orderDetails)
        {
            try
            {
                _orderRepository.Add(order);
                _unitOfWork.Commit();

                foreach (var orderDetail in orderDetails)
                {
                    orderDetail.OrderID = order.ID;
                    _orderDetailRepository.Add(orderDetail);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}