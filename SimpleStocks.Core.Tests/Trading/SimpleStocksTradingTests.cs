﻿using Moq;
using SimpleStocks.Core.DataInterface;
using SimpleStocks.Core.Domain;
using System;
using System.Collections.Generic;
using Xunit;

namespace SimpleStocks.Core.Trading
{
    public class SimpleStocksTradingTests
    {
        private readonly List<Stock> _stockList;
        private readonly Mock<ISimpleStocksRepository> _simpleStocksRepositoryMock;
        private readonly SimpleStocksTrading _trading;
        private readonly List<Trade> _tradeIntervalData;
        private TradeCalculationRequest _request;
        private Trade _tradeData;
        private readonly static int _precision = 2;

        public SimpleStocksTradingTests()
        {
            //Arrange
            _stockList = new List<Stock>
            {
               new Stock { symbol = "TEA", type = StockType.Common, lastDividend = 0, parValue = 100 },
               new Stock { symbol = "POP", type = StockType.Common, lastDividend = 8, parValue = 100 },
               new Stock { symbol = "ALE", type = StockType.Common, lastDividend = 23, parValue = 60 },
               new Stock { symbol = "GIN", type = StockType.Preferrred, lastDividend = 8, fixedDividend = 2, parValue = 100 },
               new Stock { symbol = "JOE", type = StockType.Common, lastDividend = 13, parValue = 250 }
            };

            _tradeIntervalData = new List<Trade> {

                    new Trade { symbol = "TEA", quantity = 10, price = 102, timestamp = DateTime.Now.AddMinutes(-1), tradeIndicator = TradeIndicator.BUY },
                    new Trade { symbol = "TEA", quantity = 200, price = 110, timestamp = DateTime.Now, tradeIndicator = TradeIndicator.SELL },
                    new Trade { symbol = "TEA", quantity = 10, price = 120, timestamp = DateTime.Now, tradeIndicator = TradeIndicator.BUY },
                    new Trade { symbol = "TEA", quantity = 107, price = 100, timestamp = DateTime.Now, tradeIndicator = TradeIndicator.SELL },
                    new Trade { symbol = "GIN", quantity = 10, price = 310, timestamp = DateTime.Now, tradeIndicator = TradeIndicator.BUY},
                    new Trade { symbol = "GIN", quantity = 200, price = 210, timestamp = DateTime.Now, tradeIndicator = TradeIndicator.SELL },
                    new Trade { symbol = "JOE", quantity = 10, price = 710, timestamp = DateTime.Now.AddMinutes(15), tradeIndicator = TradeIndicator.SELL },
                    new Trade { symbol = "JOE", quantity = 107, price = 180, timestamp = DateTime.Now.AddMinutes(15), tradeIndicator = TradeIndicator.BUY },

                    new Trade { symbol = "TEA", quantity = 120, price = 10, timestamp = DateTime.Now.AddMinutes(15), tradeIndicator = TradeIndicator.SELL },
                    new Trade { symbol = "ALE", quantity = 100, price = 10, timestamp = DateTime.Now, tradeIndicator = TradeIndicator.SELL },
                    new Trade { symbol = "POP", quantity = 100, price = 160, timestamp = DateTime.Now, tradeIndicator = TradeIndicator.SELL },
                    new Trade { symbol = "POP", quantity = 100, price = 10, timestamp = DateTime.Now.AddMinutes(5), tradeIndicator = TradeIndicator.SELL },

                    new Trade { symbol = "JOE", quantity = 100, price = 10, timestamp = DateTime.Now.AddMinutes(15), tradeIndicator = TradeIndicator.SELL },
                    new Trade { symbol = "POP", quantity = 100, price = 10, timestamp = DateTime.Now.AddMinutes(10), tradeIndicator = TradeIndicator.BUY },
                    new Trade { symbol = "POP", quantity = 100, price = 10, timestamp = DateTime.Now.AddMinutes(10), tradeIndicator = TradeIndicator.SELL },
                    new Trade { symbol = "GIN", quantity = 34, price = 12, timestamp = DateTime.Now.AddMinutes(10), tradeIndicator = TradeIndicator.SELL }
            };

            _simpleStocksRepositoryMock = new Mock<ISimpleStocksRepository>();
            _request = new TradeCalculationRequest();
            _trading = new SimpleStocksTrading(_stockList, _tradeIntervalData, _simpleStocksRepositoryMock.Object, _precision);
        }



        [Fact]
        public void ShouldthrowExceptionIfRequestIsNull()
        {
            _request = null;
            var exception = Assert.Throws<ArgumentNullException>(() => _trading.GetDividendYield(_request));
            Assert.Equal("RequestItem", exception.ParamName);
        }

        [Fact]
        public void ShouldReturnArgumentExceptionIfPriceIsZero()
        {
            _request.stock = "GIN";
            _request.price = 0;
            var exception = Assert.Throws<ArgumentException>(() => _trading.GetDividendYield(_request));
            Assert.Equal("price", exception.Message);
        }

        [Fact]
        public void ShouldReturnArgumentExceptionIfstockIsEmptyString()
        {
            _request.price = 10;
            var exception = Assert.Throws<ArgumentException>(() => _trading.GetDividendYield(_request));
            Assert.Equal("stock", exception.Message);
        }

        [Fact]
        public void ShouldReturnStockDoesNotExistException()
        {
            var exception = Assert.Throws<KeyNotFoundException>(() => _trading.GetDividendYield(_request));
            Assert.Equal("Stock does not exist", exception.Message);
        }


        [Fact]
        public void ShouldCalculateDividendYield()
        {

            //Act
            decimal result = _trading.GetDividendYield(_request);
            //Assert
            Assert.NotEqual(0, result);

        }

        [Fact]
        public void ShouldCalculatePERatio()
        {

            //Act
            decimal result = _trading.GetPERatio(_request);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void ShouldthrowExceptionIfTradeDataIsNull()
        {
            _tradeData = null;
            var exception = Assert.Throws<ArgumentNullException>(() => _trading.RecordTrade(_tradeData));
            Assert.Equal("tradeRequest", exception.ParamName);
            _simpleStocksRepositoryMock.Verify(x => x.Save(It.IsAny<Trade>()), Times.Never);
        }

        [Fact]
        public void ShouldSaveATrade()
        {
            Trade recordTrade = null;
            _tradeData = new Trade { symbol = "TEA", quantity = 100, price = 10, timestamp = DateTime.Now, tradeIndicator = TradeIndicator.SELL };
            _simpleStocksRepositoryMock.Setup(x => x.Save(It.IsAny<Trade>()))
                .Callback<Trade>(trade =>
                {
                    recordTrade = trade;
                });

            _trading.RecordTrade(_tradeData);

            _simpleStocksRepositoryMock.Verify(x => x.Save(It.IsAny<Trade>()), Times.Once);
            Assert.NotNull(recordTrade);
        }


        [Fact]
        public void ShouldCalculateVolumeWeightedStockPrice()
        {

            //Act
            decimal result = _trading.GetVWSP(new VWSPRequest { symbol = "TEA", timeInMinutes = 5 });

            //Assert
            Assert.NotEqual(0, result);
            Assert.Throws<ArgumentException>(() => _trading.GetVWSP(new VWSPRequest { symbol = "TEA", timeInMinutes = -1 }));
            Assert.Throws<KeyNotFoundException>(() => _trading.GetVWSP(new VWSPRequest { symbol = "TE", timeInMinutes = 5 }));
        }

        [Fact]
        public void ShouldCalculateAllShareIndex()
        {

            //Act
            double result = _trading.AllShareIndex(30);

            //Assert
            Assert.NotEqual(0, result);
            Assert.Throws<ArgumentException>(() => _trading.AllShareIndex(-30));
        }
    }
}