using CarvedRockFitnessApi.Data;
using CarvedRockFitnessApi.Models;
using System;
using System.Collections.Generic;

namespace CarvedRockFitnessApi.Services
{
    public class RevenueByCurrencyAggregator : IRevenueByCurrencyAggregator
    {
        private readonly IOrderRepository orderRepository;

        public RevenueByCurrencyAggregator(IOrderRepository orderRepository)
        {
            this.orderRepository = orderRepository;
        }

        public IEnumerable<RevenueInCurrency> GetRevenueByCurrency()
        {
            var orders = this.orderRepository.GetOrdersPlacedToday();
            var orderValueByCurrency = BucketOrderValueByCurrency(orders);

            var revenueInCurrencyList = new List<RevenueInCurrency>();
            foreach (var currencyOrderRevenue in orderValueByCurrency)
            {
                revenueInCurrencyList.Add(new RevenueInCurrency(currencyOrderRevenue.Key, currencyOrderRevenue.Value));
            }

            return revenueInCurrencyList;
        }

        private static Dictionary<Currency, decimal> BucketOrderValueByCurrency(IEnumerable<Order> orders)
        {
            var orderValueByCurrency = new Dictionary<Currency, decimal>();

            foreach (var order in orders)
            {
                if (orderValueByCurrency.ContainsKey(order.Currency))
                { 
                    orderValueByCurrency[order.Currency] += order.Price;
                }
                else
                {
                    orderValueByCurrency.Add(order.Currency, order.Price);
                }
            }

            return orderValueByCurrency;
        }
    }
}
