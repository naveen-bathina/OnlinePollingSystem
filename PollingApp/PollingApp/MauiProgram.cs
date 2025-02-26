using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using PollingApp.Pages;
using PollingApp.PageViewModels;
using PollingApp.Services;
using Syncfusion.Maui.Core.Hosting;

namespace PollingApp
{
    public static class MauiProgram
    {
        public static string API_BASE_URL = "http://10.0.2.2:8090/";
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<HttpClient>();
            builder.Services.AddSingleton<IPollApiService, PollApiService>();

            // Register ViewModels
            builder.Services.AddSingleton<PollListViewModel>();
            builder.Services.AddTransient<PollDetailViewModel>();
            builder.Services.AddTransient<PollResultViewModel>();

            // Register Pages
            builder.Services.AddSingleton<PollListPage>();
            builder.Services.AddTransient<PollDetailPage>();
            builder.Services.AddTransient<PollResultPage>();

            return builder.Build();
        }
    }
}
