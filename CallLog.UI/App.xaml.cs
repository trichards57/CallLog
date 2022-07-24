using System;
using System.Windows;
using CallLog.UI.Services;
using CallLog.UI.Services.Interfaces;
using CallLog.UI.ViewModels;
using CommunityToolkit.Mvvm.DependencyInjection;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CallLog.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void AppStartup(object sender, StartupEventArgs e)
        {
            Ioc.Default.ConfigureServices(new ServiceCollection()
                .AddSingleton(s =>
                {
                    var defaultMethodConfig = new MethodConfig
                    {
                        Names = { MethodName.Default },
                        RetryPolicy = new RetryPolicy
                        {
                            MaxAttempts = 5,
                            InitialBackoff = TimeSpan.FromSeconds(1),
                            MaxBackoff = TimeSpan.FromSeconds(5),
                            BackoffMultiplier = 1.5,
                            RetryableStatusCodes = { Grpc.Core.StatusCode.Unavailable, }
                        }
                    };

                    var channel = GrpcChannel.ForAddress("https://localhost:7244", new GrpcChannelOptions
                    {
                        ServiceConfig = new ServiceConfig { MethodConfigs = { defaultMethodConfig } }
                    });
                    return new LocalServer.Events.EventsClient(channel);
                })
                .AddSingleton<IDialogService, DialogService>()
                .AddSingleton<MainViewModel>()
                .BuildServiceProvider()
            );
        }
    }
}
