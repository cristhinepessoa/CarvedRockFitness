using CarvedRockFitnessApi.Data;
using CarvedRockFitnessApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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
            var orderValueByCurrency = GetOrderValueByCurrency(orders);

            var revenueInCurrencyList = new List<RevenueInCurrency>();
            foreach (Currency currency in Enum.GetValues(typeof(Currency)))
            {
                if (currency != Currency.Unknown)
                {
                    var currencyOrderValue = 0m;

                    if (orderValueByCurrency.ContainsKey(currency))
                    {
                        currencyOrderValue = orderValueByCurrency[currency];
                    }

                    revenueInCurrencyList.Add(new RevenueInCurrency(currency, currencyOrderValue));
                }
            }

            return revenueInCurrencyList;
        }

        public IEnumerable<RevenueInCurrency> GetRevenueByCurrencyOrdered()
        {
            var orders = this.orderRepository.GetOrdersPlacedToday();
            var orderValueByCurrency = GetOrderValueByCurrency(orders);

            var revenueInCurrencyList = new List<RevenueInCurrency>();
            foreach (Currency currency in Enum.GetValues(typeof(Currency)))
            {
                if (currency != Currency.Unknown)
                {
                    var currencyOrderValue = 0m;

                    if (orderValueByCurrency.ContainsKey(currency))
                    {
                        currencyOrderValue = orderValueByCurrency[currency];
                    }

                    revenueInCurrencyList.Add(new RevenueInCurrency(currency, currencyOrderValue));
                }
            }

            return revenueInCurrencyList.OrderByDescending(keySelector:currencyRevenue => currencyRevenue.Revenue);
        }

        private static Dictionary<Currency, decimal> GetOrderValueByCurrency(IEnumerable<Order> orders)
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
