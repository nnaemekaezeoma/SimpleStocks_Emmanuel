using SimpleStocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStocks.Core.Logic
{
    class AllShareIndex
    {
        public double CalculateAllShareIndex(List<Trade> tradeData, List<Stock> stocks, int cutofftimeInMinutes)
        {
            int n = 0;
            double root = 0, allShareIndex = 0;
            decimal VMPrice = 1;
            List<Trade> stockTrade = new List<Trade>();
            VolumeWeightedStockPrice _VWPrice = new VolumeWeightedStockPrice();
            foreach (Stock stock in stocks)
            {
                stockTrade = Helper.IntervalStockTradedIntimePeriod(tradeData, stock.symbol, cutofftimeInMinutes);
                if (stockTrade.Count > 0)
                {
                    //get volume Weighted stock price and multiply with the previous stock volume Weighted stock price
                    VMPrice *= _VWPrice.CalculateVolumeWeightedStockPrice(stockTrade);
                    n++;
                }
            }
            if (n == 0)
            {
                throw new ArithmeticException();
            }
            root = (double)Decimal.Divide(1, n);
            allShareIndex = Math.Pow((double)VMPrice, root);
            return allShareIndex;
        }
    }
}
