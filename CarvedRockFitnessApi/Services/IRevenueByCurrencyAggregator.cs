using CarvedRockFitnessApi.Models;
using System.Collections.Generic;

namespace CarvedRockFitnessApi.Services
{
    public interface IRevenueByCurrencyAggregator
    {
        IEnumerable<RevenueInCurrency> GetRevenueByCurrencyOrdered();
    }
}
