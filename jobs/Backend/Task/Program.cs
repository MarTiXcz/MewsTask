﻿using ExchangeRateUpdater.Abstractions;
using ExchangeRateUpdater.Data;
using ExchangeRateUpdater.ExchangeRateSources.CNB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace ExchangeRateUpdater;

public sealed class Program
{
    public static void Main(string[] args)
    {
        try
        {
            using IHost host = CreateHostBuilder(args).Build();
            host.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Could not retrieve exchange rates: '{e.Message}'.");
        }
    }
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
        .ConfigureServices(ConfigureServices)
        .ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
        });

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services) =>
            services
                .Configure<CurrencyOptions>(context.Configuration.GetSection("CurrencyOptions") ?? throw new Exception("Missing currency options"))
                .Configure<CNBSourceOptions>(context.Configuration.GetSection("CNBSourceOptions") ?? throw new Exception("Missing CNB source options"))
                .AddSingleton<IExchangeRateSource, CNBExchangeRateSource>()
                .AddTransient<IExchangeRateProvider, ExchangeRateProvider>()
                .AddTransient<IExchangeRatePrinter, ExchangeRatePrinter>()
                .AddHostedService<ExchangeRateService>();

}
