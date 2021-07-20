using SimpleStocks.Core.Domain;
using System;
using System.Collections.Generic;

namespace SimpleStocks.Core.Logic
{
    public class DividendYield
    {
        //Calculate Yield
        public decimal CalculateYield(Stock request, int price)
        {
            decimal yield = 0;

            if (request.type == StockType.Common)
            {
                yield = Decimal.Divide(request.lastDividend, price);
            }
            else if (request.type == StockType.Preferrred)
            {
                decimal percent = Decimal.Divide((decimal)request.fixedDividend, 100);
                yield = Decimal.Divide((percent * request.parValue), price);
            }

            return yield;
        }
    }
}
