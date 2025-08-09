using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using DesktopApp.Views;

namespace DesktopApp;

public partial class App : Application
{
    private WebAppMetadata? _application = null;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        _application = WebApplicationCreator.CreateWebApp();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow(_application.FrontendUri);

            desktop.MainWindow = mainWindow;
            desktop.Exit += OnExit;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private async void OnExit(object? sender, ControlledApplicationLifetimeExitEventArgs e)
    {
        if (_application is not null)
        {
            await _application.WebApplication.StopAsync();
            await _application.WebApplication.DisposeAsync();
        }
    }

}


