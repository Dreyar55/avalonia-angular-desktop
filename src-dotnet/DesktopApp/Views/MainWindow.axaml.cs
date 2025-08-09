using Avalonia.Controls;

namespace DesktopApp.Views;

public partial class MainWindow : Window
{
    public MainWindow(Uri frontendAddress)
    {
        InitializeComponent();

        WebViewControl.Url = frontendAddress;
        CanResize = true;
        WindowState = WindowState.Maximized;
    }

}