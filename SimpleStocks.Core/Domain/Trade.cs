using System;
using System.ComponentModel.DataAnnotations;

namespace SimpleStocks.Core.Domain
{
    public class Trade : StockBase
    {
       
        public int quantity { get; set; }
        public DateTime timestamp { get; set; }
 
        public decimal price { get; set; }

        public TradeIndicator tradeIndicator { get; set; }
    }
}