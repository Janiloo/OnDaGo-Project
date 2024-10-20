using Android.App;
using Android.Runtime;

namespace Maui.Onion
{
    [Application]
    [MetaData("com.google.android.maps.v2.API_KEY",
        Value = "AIzaSyBNWpBKVmqLmE3-nP9sKuQHTN8Jn40pN4w")]
    public class MainApplication : MauiApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }

        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    }
}
