using SimpleStocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleStocks.Core.Logic
{
    public class VolumeWeightedStockPrice
    {
        //Calculates WeightedTrade
        public decimal CalculateVolumeWeightedStockPrice(List<Trade> trades)
        {
            decimal SumTradedPriceQuantity = 0, SumQuantity = 0;

            foreach (Trade item in trades)
            {
                SumTradedPriceQuantity += (item.price * item.quantity);
                SumQuantity += item.quantity;
            }
            return Decimal.Divide(SumTradedPriceQuantity, SumQuantity);
        }

    }
}
