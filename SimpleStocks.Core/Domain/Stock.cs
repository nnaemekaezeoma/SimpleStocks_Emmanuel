using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStocks.Core.Domain
{
    public class Stock : StockBase
    {
        public StockType type { get; set; }
        public int lastDividend { get; set; }
        public int? fixedDividend { get; set; } = null;
        public int parValue { get; set; }
    }
}
