using SimpleStocks.Core.DataInterface;
using SimpleStocks.Core.Domain;
using SimpleStocks.Core.Logic;
using System;
using System.Collections.Generic;

namespace SimpleStocks.Core.Trading
{
    public class SimpleStocksTrading
    {
        private static int _precision;
        private List<Stock> _stocks;
        private readonly List<Trade> _tradeData;
        private readonly ISimpleStocksRepository _stockRepository;

        public SimpleStocksTrading(List<Stock> stocks, List<Trade> tradeData, ISimpleStocksRepository simpleStockRepository, int precision)
        {
            _stocks = stocks;
            _tradeData = tradeData;
            _precision = precision;
            _stockRepository = simpleStockRepository;
        }

        //Get dividend yield 
        public decimal GetDividendYield(TradeCalculationRequest request)
        {
            decimal yield = 0;
            DividendYield dividendYield = new DividendYield();
            if (Helper.ValidRequest(request))
            {
                Stock stock = Helper.GetStock(_stocks, request.stock);
                yield = dividendYield.CalculateYield(stock, request.price);
            }
            return Math.Round(yield, _precision);
        }

        //get PE Ratio
        public decimal GetPERatio(TradeCalculationRequest request)
        {
            decimal peRatio = 0;
            PERatio ratio = new PERatio();
            if (Helper.ValidRequest(request))
            {
                Stock stock = Helper.GetStock(_stocks, request.stock);
                peRatio = ratio.CalculatePERatio(stock.lastDividend, request.price);
            }
            return Math.Round(peRatio, _precision);
        }

        //Get Volume Weighted Stock Price
        public decimal GetVWSP(VWSPRequest request)
        {
            decimal vWeightedStockPrice = 0;
            VolumeWeightedStockPrice VWPrice = new VolumeWeightedStockPrice();

            if (request.timeInMinutes < 0)
            {
                throw new ArgumentException();
            }

            List<Trade> stockTrade = Helper.IntervalStockTradedIntimePeriod(_tradeData, request.symbol, request.timeInMinutes);
            if (stockTrade.Count == 0)
            {
                throw new KeyNotFoundException();
            }

            vWeightedStockPrice = VWPrice.CalculateVolumeWeightedStockPrice(stockTrade);
            //round to provided precision
            return Math.Round(vWeightedStockPrice, _precision);
        }

        //Get all Share Index
        public double GetAllShareIndex(int cutofftimeInMinutes)
        {
            AllShareIndex data = new AllShareIndex();
            if (cutofftimeInMinutes <= 0)
            {
                throw new ArgumentException();
            }

            double allShareIndex = data.CalculateAllShareIndex(_tradeData, _stocks, cutofftimeInMinutes);
            //round to provided precision
            return Math.Round(allShareIndex, _precision);
        }

        //Record a trade
        public void RecordTrade(Trade tradeData)
        {
            if (Helper.ValidTradeRequest(tradeData))
            {
                _stockRepository.Save(tradeData);
            }
        }

    }
}