using SimpleStocks.Core.Domain;
using System;
using System.Collections.Generic;

namespace SimpleStocks.Core.Logic
{
    class PERatio
    {
        public decimal CalculatePERatio(int dividend, int price)
        {
            if (dividend.Equals(0))
            {
                throw new DivideByZeroException();
            }
           return Decimal.Divide(price, dividend);
   
        }
    }
}
