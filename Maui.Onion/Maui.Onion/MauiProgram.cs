using Maui.GoogleMaps.Hosting;
using Maui.GoogleMaps;
using Microsoft.Extensions.Logging;

namespace Maui.Onion
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
#if ANDROID
builder.UseGoogleMaps();

#elif IOS
builder.UseGoogleMaps("AIzaSyBNWpBKVmqLmE3-nP9sKuQHTN8Jn40pN4w");
#endif

            return builder.Build();
        }
    }
}
