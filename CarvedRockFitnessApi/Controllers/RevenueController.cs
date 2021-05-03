using CarvedRockFitnessApi.Models;
using CarvedRockFitnessApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace CarvedRockFitnessApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RevenueController : Controller
    {
        private readonly IRevenueByCurrencyAggregator _revenueByCurrencyAggregator;

        public RevenueController(IRevenueByCurrencyAggregator revenueByCurrencyAggregator)
        {
            this._revenueByCurrencyAggregator = revenueByCurrencyAggregator;
        }

        [HttpGet]
        public IEnumerable<RevenueInCurrency> Index()
        {
            return this._revenueByCurrencyAggregator.GetRevenueByCurrencyOrdered();
        }
    }
}
