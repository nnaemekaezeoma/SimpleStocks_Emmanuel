using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStocks.Core.Domain
{
    public class TradeCalculationRequest
    {
        public string stock { get; set; }
        public int price { get; set; }
    }
}
