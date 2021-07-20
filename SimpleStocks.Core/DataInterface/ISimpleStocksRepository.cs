using SimpleStocks.Core.Domain;

namespace SimpleStocks.Core.DataInterface
{
    public interface ISimpleStocksRepository
    {
        void Save(Trade tradingData);
    }
}
