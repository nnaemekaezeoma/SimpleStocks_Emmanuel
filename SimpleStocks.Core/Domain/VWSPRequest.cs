using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStocks.Core.Domain
{
    public class VWSPRequest: StockBase
    {
        public int timeInMinutes { get; set; }
    }
}
