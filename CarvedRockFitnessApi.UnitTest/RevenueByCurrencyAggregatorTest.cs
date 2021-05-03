using CarvedRockFitnessApi.Data;
using CarvedRockFitnessApi.Models;
using CarvedRockFitnessApi.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarvedRockFitnessApi.UnitTest
{
    [TestFixture]
    public class RevenueByCurrencyAggregatorTest
    {
        Mock<IOrderRepository> orderRepository;
        RevenueByCurrencyAggregator revenueByCurrencyAggregator;

        [SetUp]
        public void Setup()
        {
            this.orderRepository = new Mock<IOrderRepository>();
            this.revenueByCurrencyAggregator = new RevenueByCurrencyAggregator(this.orderRepository.Object);
        }

        [Test]
        public void GetRevenueByCurrency_OneEurOrder_CorrectRevenueForEur()
        {
            this.orderRepository.Setup(repository => repository.GetOrdersPlacedToday()).Returns(new List<Order>()
            {
                new Order(Currency.Eur, 100m)
            });

            var result = this.revenueByCurrencyAggregator.GetRevenueByCurrency();

            Assert.AreEqual(100m, result.ToList().FirstOrDefault(currencyByRevenue => currencyByRevenue.Currency == Currency.Eur)?.Revenue);
        }

        [Test]
        public void GetRevenueByCurrency_TwoEurOrder_CorrectRevenueForEur()
        {
            this.orderRepository.Setup(repository => repository.GetOrdersPlacedToday()).Returns(new List<Order>()
            {
                new Order(Currency.Eur, 200m),
                new Order(Currency.Eur, 100m)
            });

            var result = this.revenueByCurrencyAggregator.GetRevenueByCurrency();

            Assert.AreEqual(300m, result.ToList().FirstOrDefault(currencyByRevenue => currencyByRevenue.Currency == Currency.Eur)?.Revenue);
        }

        [Test]
        public void GetRevenueByCurrency_NoUsdOrders_UsdRevenueZero()
        {
            this.orderRepository.Setup(repository => repository.GetOrdersPlacedToday()).Returns(new List<Order>()
            {
                new Order(Currency.Eur, 200m),
                new Order(Currency.Eur, 100m)
            });

            var result = this.revenueByCurrencyAggregator.GetRevenueByCurrency();

            Assert.AreEqual(0, result.ToList().FirstOrDefault(currencyByRevenue => currencyByRevenue.Currency == Currency.Usd)?.Revenue);
        }


        [Test]
        public void GetRevenueByCurrency_NoUnkownCurrenncyOrders_UnkownCurrencyRevenueNull()
        {
            this.orderRepository.Setup(repository => repository.GetOrdersPlacedToday()).Returns(new List<Order>()
            {
                new Order(Currency.Eur, 200m),
                new Order(Currency.Usd, 100m)
            });

            var result = this.revenueByCurrencyAggregator.GetRevenueByCurrency();

            Assert.Null(result.ToList().FirstOrDefault(currencyByRevenue => currencyByRevenue.Currency == Currency.Unknown)?.Revenue);
        }

        [Test]
        public void GetRevenueByCurrencyOrdered_NoUnkownCurrenncyOrders_ReorderByRevenue()
        {
            this.orderRepository.Setup(repository => repository.GetOrdersPlacedToday()).Returns(new List<Order>()
            {
                new Order(Currency.Gbp, 10m),
                new Order(Currency.Eur, 100m)
            });

            var result = this.revenueByCurrencyAggregator.GetRevenueByCurrencyOrdered();
            var expected = new[] { 100m, 10m, 0m };

            Assert.IsTrue(result.Select(res => res.Revenue).SequenceEqual(expected));
        }
    }
}
