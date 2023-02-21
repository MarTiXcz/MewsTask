﻿using ExchangeRateUpdater.CNB;

namespace ExchangeRateUpdater.Tests.ExchangeRateSources.CNB;

public class CNBExchangeRateParserTests
{
    [Fact()]
    public void ParseRatesTest()
    {
        var uri = "../../../Data/denni_kurz.txt";
        var allFileText = File.ReadAllText(uri);
        var rates = CNBExchangeRateParser.ParseRates(allFileText);
        Assert.Equal(31, rates.Count());
    }

    [Fact()]
    public void ParseRatesInvalidFormatTest()
    {
        var allFileText = "CZK 1,5";
        Assert.Throws<FormatException>(() =>
        {
            var rates = CNBExchangeRateParser.ParseRates(allFileText).ToList();
        });
    }
}