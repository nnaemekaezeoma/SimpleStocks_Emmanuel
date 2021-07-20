using SimpleStocks.Core.Domain;
using System;
using System.Collections.Generic;

namespace SimpleStocks.Core.Logic
{
    class PERatio
    {
        public decimal CalculatePERatio(int dividend, int price)
        {
           return Decimal.Divide(price, dividend);
   
        }
    }
}
