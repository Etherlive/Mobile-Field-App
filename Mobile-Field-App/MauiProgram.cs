using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebView.Maui;
using Mobile_Field_App.Data;

namespace Mobile_Field_App;

public static class MauiProgram
{
    #region Methods

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });

        builder.Services.AddMauiBlazorWebView();
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
#endif
        builder.Services.AddSingleton<AuthenticationManager>();

        API_Whisperer.Request.urlStart = "https://portalapi.etherlive.com/api/";

        return builder.Build();
    }

    #endregion Methods
}