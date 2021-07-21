using SimpleStocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SimpleStocks.Core.Logic
{
    public class Helper
    {
        //Validates Trade calculation request item
        public static bool ValidRequest(TradeCalculationRequest RequestItem)
        {
            if (RequestItem == null)
            {
                throw new ArgumentNullException(nameof(RequestItem));
            }

            if (string.IsNullOrEmpty(RequestItem.stock))
            {
                throw new ArgumentException(nameof(RequestItem.stock));
            }

            if (RequestItem.price <= 0)
            {
                throw new ArgumentException(nameof(RequestItem.price));
            }

            else
            {
                return true;
            }
        }

        //validate trade record request
        public static bool ValidTradeRequest(Trade tradeRequest)
        {
            if (tradeRequest == null)
            {
                throw new ArgumentNullException(nameof(tradeRequest));
            }

            ValidationContext vc = new ValidationContext(tradeRequest);
            List<ValidationResult> results = new List<ValidationResult>(); 
            bool isValid = Validator.TryValidateObject(tradeRequest, vc, results); 

            if (!isValid)
            {
                string message = string.Empty;
                foreach(ValidationResult res in results)
                {
                    message += res.MemberNames.FirstOrDefault() + ": " + res.ErrorMessage + ", ";
                }
                throw new ArgumentException(message);
            }
            else
            {
                return true;
            }
            
        }

        //returns the requested stock from the stocks 
        public static Stock GetStock(List<Stock> stocks, string stock)
        {
            Stock stock1 = stocks.Where(stockItem => stockItem.symbol == stock).FirstOrDefault();

            if (stock1 == null)
            {
                throw new KeyNotFoundException("Stock does not exist");
            }
            return stock1;
        }

        //get provided stock and trade interval 
        public static List<Trade> IntervalStockTradedIntimePeriod(List<Trade> trades, string stock, int intervalMinute)
        {
            if (trades == null)
            {
                throw new ArgumentNullException();
            }

            DateTime endtime = DateTime.Now;
            DateTime startTime = DateTime.Now.AddMinutes(-intervalMinute);
            return trades.Where(trade => (trade.symbol == stock && (trade.timestamp >= startTime && trade.timestamp <= endtime))).ToList();

        }

    }
}
